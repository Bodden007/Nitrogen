using NModbus;
using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Nitrogen.Services.Modbus.Connection;

internal sealed class ModbusConnectionManager : IModbusReader, IModbusWriter, IDisposable
{
    private readonly ModbusConnectionConfig _config;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private readonly SemaphoreSlim _requestLock = new(1, 1);
    private DateTime _lastConnectAttemptUtc = DateTime.MinValue;

    private TcpClient? _tcpClient;
    private IModbusMaster? _master;

    /// <summary>
    /// DIAGNOSTICS
    /// </summary>
    public long RequestLockWaitCount { get; private set; }
    public long RequestLockEnterCount { get; private set; }
    public long RequestStartedCount { get; private set; }
    public long RequestCompletedCount { get; private set; }
    public long HungRequestCount { get; private set; }
    public long RecoveredRequestCount { get; private set; }

    public bool RequestInProgress { get; private set; }
    public bool RequestIsHung { get; private set; }
    public DateTime? CurrentRequestStartedAt { get; private set; }
    public long CurrentRequestMs { get; private set; }
    public long LastRequestMs { get; private set; }
    public long MaxRequestMs { get; private set; }
    public DateTime? LastSuccessTime { get; private set; }
    /// <summary>
    /// END DIAGNOSTICS
    /// </summary>

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
        ushort[]? registers = await ExecuteRequestAsync(
            master => master.ReadInputRegistersAsync(
                slaveId,
                startAddress,
                count),
            cancellationToken);

        return registers ?? Array.Empty<ushort>();
    }

    public async Task WriteSingleRegisterAsync(
        byte slaveId,
        ushort address,
        ushort value,
        CancellationToken cancellationToken = default)
    {
        await ExecuteRequestAsync(
            master => master.WriteSingleRegisterAsync(
                slaveId,
                address,
                value),
            cancellationToken);
    }

    public async Task WriteMultipleRegistersAsync(
        byte slaveId,
        ushort startAddress,
        ushort[] values,
        CancellationToken cancellationToken = default)
    {
        await ExecuteRequestAsync(
            master => master.WriteMultipleRegistersAsync(
                slaveId,
                startAddress,
                values),
            cancellationToken);
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

        //FIXMI Diagnostics
        RequestLockWaitCount++;

        await _requestLock.WaitAsync(cancellationToken);

        //FIXMI Diagnostics
        RequestLockEnterCount++;

        try
        {
            //FIXMI Diagnostics
            /* await request(_master)*/
            ;
            await ExecuteRequestWithDiagnosticsAsync(
                    () => request(_master));
        }
        catch
        {
            CloseConnection();
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

        //FIXMI Diagnostics
        RequestLockWaitCount++;

        await _requestLock.WaitAsync(cancellationToken);

        //FIXMI Diagnostics
        RequestLockEnterCount++;

        try
        {
            //FIXMI Diagnostics
            //return await request(_master);
            return await ExecuteRequestWithDiagnosticsAsync(
                        () => request(_master));
        }
        catch
        {
            CloseConnection();
            return default;
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

    private async Task ExecuteRequestWithDiagnosticsAsync(Func<Task> request)
    {
        await ExecuteRequestWithDiagnosticsAsync(
            async () =>
            {
                await request();
                return true;
            });
    }

    //FIXMI DIANOSTICS
    private async Task<TResult> ExecuteRequestWithDiagnosticsAsync<TResult>(
        Func<Task<TResult>> request)
    {
        RequestStartedCount++;
        RequestInProgress = true;
        RequestIsHung = false;
        CurrentRequestStartedAt = DateTime.Now;
        CurrentRequestMs = 0;

        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            Task<TResult> requestTask = request();

            Task timeoutTask = Task.Delay(_config.RequestTimeoutMs);

            Task completedTask = await Task.WhenAny(requestTask, timeoutTask);

            if (completedTask != requestTask)
            {
                CurrentRequestMs = stopwatch.ElapsedMilliseconds;
                LastRequestMs = stopwatch.ElapsedMilliseconds;
                MaxRequestMs = Math.Max(MaxRequestMs, LastRequestMs);

                HungRequestCount++;
                RequestIsHung = true;

                throw new TimeoutException(
                    $"Modbus request hung longer than {_config.RequestTimeoutMs} ms");
            }

            TResult result = await requestTask;

            stopwatch.Stop();

            CurrentRequestMs = stopwatch.ElapsedMilliseconds;
            LastRequestMs = stopwatch.ElapsedMilliseconds;
            MaxRequestMs = Math.Max(MaxRequestMs, LastRequestMs);
            LastSuccessTime = DateTime.Now;

            RequestCompletedCount++;

            return result;
        }
        finally
        {
            RequestInProgress = false;
            RequestIsHung = false;
            CurrentRequestStartedAt = null;
        }
    }
}