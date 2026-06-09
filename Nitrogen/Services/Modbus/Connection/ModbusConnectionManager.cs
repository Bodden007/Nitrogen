using NModbus;
using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Nitrogen.Services.Modbus.Connection;

internal sealed class ModbusConnectionManager : IModbusReader, IModbusWriter, IDisposable
{
    private readonly ModbusConnectionConfig _config;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private readonly SemaphoreSlim _requestLock = new(1, 1);
    private DateTime _lastConnectAttemptUtc = DateTime.MinValue;

    private TcpClient? _tcpClient;
    private IModbusMaster? _master;

    public ModbusConnectionManager(ModbusConnectionConfig config)
    {
        _config = config;
    }

    public async Task<ushort[]> ReadInputRegistersAsync(
       byte slaveId,
       ushort startAddress,
       ushort count,
       CancellationToken cancellationToken = default)
    {
        try
        {
            ushort[]? registers = await ExecuteRequestAsync(
                master => master.ReadInputRegistersAsync(
                    slaveId,
                    startAddress,
                    count),
                cancellationToken);

            return registers ?? Array.Empty<ushort>();
        }
        catch
        {
            CloseConnection();

            return Array.Empty<ushort>();
        }
    }

    public async Task WriteSingleRegisterAsync(
        byte slaveId,
        ushort address,
        ushort value,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await ExecuteRequestAsync(
                master => master.WriteSingleRegisterAsync(
                    slaveId,
                    address,
                    value),
                cancellationToken);
        }
        catch
        {
            CloseConnection();
        }
    }

    public async Task WriteMultipleRegistersAsync(
        byte slaveId,
        ushort startAddress,
        ushort[] values,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await ExecuteRequestAsync(
                master => master.WriteMultipleRegistersAsync(
                    slaveId,
                    startAddress,
                    values),
                cancellationToken);
        }
        catch
        {
            CloseConnection();
        }
    }

    private async Task<bool> EnsureConnectedAsync(CancellationToken cancellationToken)
    {
        if (_master is not null && _tcpClient?.Connected == true)
            return true;

        await _connectionLock.WaitAsync(cancellationToken);

        try
        {
            if (_master is not null && _tcpClient?.Connected == true)
                return true;

            TimeSpan reconnectDelay = TimeSpan.FromMilliseconds(_config.ReconnectDelayMs);
            TimeSpan elapsed = DateTime.UtcNow - _lastConnectAttemptUtc;

            if (_lastConnectAttemptUtc != DateTime.MinValue && elapsed < reconnectDelay)
                return false;

            _lastConnectAttemptUtc = DateTime.UtcNow;

            CloseConnection();

            TcpClient tcpClient = new();

            using CancellationTokenSource timeoutCts =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            timeoutCts.CancelAfter(_config.ConnectTimeoutMs);

            await tcpClient.ConnectAsync(
                _config.Host,
                _config.Port,
                timeoutCts.Token);

            ModbusFactory factory = new();

            _tcpClient = tcpClient;
            _master = factory.CreateMaster(tcpClient);

            return true;
        }
        catch
        {
            CloseConnection();
            return false;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    private async Task ExecuteRequestAsync(
    Func<IModbusMaster, Task> request,
    CancellationToken cancellationToken)
    {
        bool connected = await EnsureConnectedAsync(cancellationToken);

        if (!connected || _master is null)
            return;

        await _requestLock.WaitAsync(cancellationToken);

        try
        {
            await request(_master);
        }
        finally
        {
            _requestLock.Release();
        }
    }

    private async Task<TResult?> ExecuteRequestAsync<TResult>(
    Func<IModbusMaster, Task<TResult>> request,
    CancellationToken cancellationToken)
    {
        bool connected = await EnsureConnectedAsync(cancellationToken);

        if (!connected || _master is null)
            return default;

        await _requestLock.WaitAsync(cancellationToken);

        try
        {
            return await request(_master);
        }
        finally
        {
            _requestLock.Release();
        }
    }

    private void CloseConnection()
    {
        _master?.Dispose();
        _master = null;

        _tcpClient?.Dispose();
        _tcpClient = null;
    }

    public void Dispose()
    {
        CloseConnection();

        _connectionLock.Dispose();
        _requestLock.Dispose();
    }
}