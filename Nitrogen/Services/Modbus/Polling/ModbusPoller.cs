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
            .Interval(TimeSpan.FromMilliseconds(500))
            .StartWith(0)
            .Subscribe(_ =>
            {
                // TODO: заменить на реальное чтение Modbus
                ushort[] registers = new ushort[50];

                _registers.OnNext(registers);
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