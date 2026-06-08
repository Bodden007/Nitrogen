using System;
using System.Collections.Generic;
using System.Text;

namespace Nitrogen.Services.Modbus.Rx
{
    internal interface IModbusRxService : IDisposable
    {
        IObservable<ushort[]> Registers { get; }
        void Start();
        void Stop();
    }
}
