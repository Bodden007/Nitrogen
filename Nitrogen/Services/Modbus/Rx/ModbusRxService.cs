using Nitrogen.Services.Modbus.Mapping;
using Nitrogen.Services.Modbus.Polling;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Nitrogen.Services.Modbus.Rx;

internal sealed class ModbusRxService : IModbusRxService
{
    private readonly IModbusPoller _poller;
    private readonly ModbusProcessValueBuilder _processValueBuilder;

    public ModbusRxService(
    IModbusPoller poller,
    ModbusProcessValueBuilder processValueBuilder)
    {
        _poller = poller;
        _processValueBuilder = processValueBuilder;
    }

    public IObservable<ushort[]> Registers => _poller.Registers;

    public IObservable<IReadOnlyDictionary<string, ProcessValue>> ProcessValues =>
        _poller.Registers
            .Select(registers =>
            {
                var values = _processValueBuilder.Build(registers);

                //FIXME Диагностика Pressure_1 RX
                if (values.TryGetValue("Pressure_1", out var pressure))
                {
                    Console.WriteLine(
                        $"RX Pressure_1 Value={pressure.Value} HasError={pressure.HasError} ErrorText={pressure.ErrorText}");
                }
                else
                {
                    Console.WriteLine("RX Pressure_1 NOT FOUND");
                }

                return values;
            });

    public void Start()
    {
        _poller.Start();
    }
    public void Stop()
    {
        _poller.Stop();
    }
    public void Dispose()
    {
        _poller.Dispose();
    }
}