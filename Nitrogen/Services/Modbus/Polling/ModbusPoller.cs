using Microsoft.Win32;
using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using Nitrogen.Services.Modbus.Connection;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Nitrogen.Services.Modbus.Polling;

internal sealed class ModbusPoller : IModbusPoller
{
    private readonly IModbusReader _reader;
    private readonly ModbusConnectionConfig _config;
    private readonly Subject<ushort[]> _registers = new();
    private IDisposable? _pollingSubscription;

    public IObservable<ushort[]> Registers => _registers.AsObservable();

    public bool IsRunning => _pollingSubscription != null;
    public ModbusPoller(
        IModbusReader reader,
        ModbusConnectionConfig config)
    {
        _reader = reader;
        _config = config;
    }

    public void Start()
    {
        if (IsRunning)
            return;

        _pollingSubscription = Observable
            .Interval(TimeSpan.FromMilliseconds(_config.PollIntervalMs))
            .StartWith(0)
            .SelectMany(async _ =>
            {
                //FIXME Диагностика
                Console.WriteLine($"POLL TICK {DateTime.Now:HH:mm:ss}");

                ushort[] registers = await _reader.ReadInputRegistersAsync(
                    _config.SlaveId,
                    _config.InputStartAddress,
                    _config.InputRegisterCount);

                return registers;
            })
            .Where(registers => registers.Length > 0)
            .Subscribe(
                registers =>
                {

                    _registers.OnNext(registers);
                },
                ex =>
                {
                    Console.WriteLine($"POLL ERROR {DateTime.Now:HH:mm:ss}: {ex}");
                });
    }

    public void Stop()
    {
        _pollingSubscription?.Dispose();
        _pollingSubscription = null;
    }

    public void Dispose()
    {
        Stop();
        _registers.Dispose();
    }
}