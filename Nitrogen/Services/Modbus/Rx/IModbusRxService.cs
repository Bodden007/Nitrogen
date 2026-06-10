using System;
using System.Collections.Generic;
using Nitrogen.Services.Modbus.Mapping;

namespace Nitrogen.Services.Modbus.Rx
{
    internal interface IModbusRxService : IDisposable
    {
        IObservable<ushort[]> Registers { get; }
        IObservable<IReadOnlyDictionary<string, ProcessValue>> ProcessValues { get; }
        void Start();
        void Stop();
    }
}
