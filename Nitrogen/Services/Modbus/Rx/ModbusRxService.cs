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
            .Select(registers => _processValueBuilder.Build(registers));

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