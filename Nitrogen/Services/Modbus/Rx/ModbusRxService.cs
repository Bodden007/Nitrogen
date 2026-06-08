using Nitrogen.Services.Modbus.Polling;
using System;

namespace Nitrogen.Services.Modbus.Rx;

internal sealed class ModbusRxService : IModbusRxService
{
    private readonly IModbusPoller _poller;
    public ModbusRxService(IModbusPoller poller)
    {
        _poller = poller;
    }
    public IObservable<ushort[]> Registers => _poller.Registers;
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