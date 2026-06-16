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

    #region DIAGNOSTICS

    private IDisposable? _diagnosticsSubscription;

    public long TickCount { get; private set; }
    public long ReadCallStartedCount { get; private set; }
    public long ReadCallCompletedCount { get; private set; }
    public long RegistersPublishedCount { get; private set; }

    public DateTime? LastTickTime { get; private set; }
    public DateTime? LastReadCompletedTime { get; private set; }
    public DateTime? LastPublishedTime { get; private set; }

    #endregion

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
                #region DIAGNOSTICS

                TickCount++;
                LastTickTime = DateTime.Now;
                ReadCallStartedCount++;

                #endregion

                ushort[] registers = await _reader.ReadInputRegistersAsync(
                    _config.SlaveId,
                    _config.InputStartAddress,
                    _config.InputRegisterCount);

                #region DIAGNOSTICS

                ReadCallCompletedCount++;
                LastReadCompletedTime = DateTime.Now;

                #endregion

                return registers;
            })
            .Where(registers => registers.Length > 0)
            .Subscribe(
                registers =>
                {
                    #region DIAGNOSTICS

                    RegistersPublishedCount++;
                    LastPublishedTime = DateTime.Now;

                    #endregion

                    _registers.OnNext(registers);
                },
                ex =>
                {
                    Console.WriteLine($"POLL ERROR {DateTime.Now:HH:mm:ss}: {ex}");
                });

        #region DIAGNOSTICS

        _diagnosticsSubscription = Observable
            .Interval(TimeSpan.FromSeconds(5))
            .Subscribe(_ =>
            {
                Console.WriteLine(
                    $"DIAG POLL " +
                    $"T={TickCount} " +
                    $"RS={ReadCallStartedCount} " +
                    $"RC={ReadCallCompletedCount} " +
                    $"PUB={RegistersPublishedCount} " +
                    $"LT={FormatTime(LastTickTime)} " +
                    $"LRC={FormatTime(LastReadCompletedTime)} " +
                    $"LP={FormatTime(LastPublishedTime)}");
            });

        #endregion
    }

    public void Stop()
    {
        _pollingSubscription?.Dispose();
        _pollingSubscription = null;

        #region DIAGNOSTICS

        _diagnosticsSubscription?.Dispose();
        _diagnosticsSubscription = null;

        #endregion
    }

    public void Dispose()
    {
        Stop();
        _registers.Dispose();
    }

    #region DIAGNOSTICS

    private static string FormatTime(DateTime? value)
    {
        return value?.ToString("HH:mm:ss") ?? "--:--:--";
    }

    #endregion
}