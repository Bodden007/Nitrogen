using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nitrogen.Services.Modbus.Polling
{
    internal class ModbusPoller : IModbusPoller
    {
        private CancellationTokenSource? _cts;
        private bool _isRunning;

        public event Action<ushort[]>? RegistersReceived;

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public void Start()
        {
            if (_isRunning)
                return;

            _isRunning = true;
            _cts = new CancellationTokenSource();

            Task.Run(() => PollLoop(_cts.Token));
        }

        public void Stop()
        {
            _isRunning = false;

            if (_cts != null)
                _cts.Cancel();
        }

        private async Task PollLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // Временно имитируем чтение Modbus
                ushort[] fakeRegisters = new ushort[50];

                RegistersReceived?.Invoke(fakeRegisters);

                await Task.Delay(500, token);
            }
        }

        public void Dispose()
        {
            Stop();

            if (_cts != null)
                _cts.Dispose();
        }
    }
}