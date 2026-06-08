using System;
using System.Collections.Generic;
using System.Text;

namespace Nitrogen.Services.Modbus.Polling
{
    internal interface IModbusPoller : IDisposable
    {
        event Action<ushort[]>? RegistersReceived;

        bool IsRunning { get; }

        void Start();

        void Stop();
    }
}
