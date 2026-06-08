using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Nitrogen.Services.Modbus.Polling;

internal sealed class ModbusPoller : IModbusPoller
{
    private readonly Subject<ushort[]> _registers = new();
    private IDisposable? _pollingSubscription;

    public IObservable<ushort[]> Registers => _registers.AsObservable();

    public bool IsRunning => _pollingSubscription != null;

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