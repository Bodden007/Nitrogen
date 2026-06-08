using Nitrogen.Services.Modbus.Rx;
using Nitrogen.Services.SystemTime;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace Nitrogen.Views.MainWindow
{
    internal class MainWindowViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; } = new();

        private readonly ObservableAsPropertyHelper<DateTime> _currentTime;

        private readonly ObservableAsPropertyHelper<ushort[]> _registers;

        public MainWindowViewModel(ISystemTimeService systemTimeService, IModbusRxService modbusRxService)
        {
            _currentTime = systemTimeService.Ticks
                .ToProperty(this, vm => vm.CurrentTime, initialValue: DateTime.MinValue);

            var registersStream = modbusRxService.Registers
                .ObserveOn(RxSchedulers.MainThreadScheduler);

            _registers = registersStream
                .ToProperty(this, vm => vm.Registers, initialValue: new ushort[50]);

            //FIXME Диагностика Rx
            registersStream
                .Subscribe(x =>
                {
                    Console.WriteLine($"RX: {x.Length}");
                });

            modbusRxService.Start();


        }
        public DateTime CurrentTime => _currentTime.Value;

        public ushort[] Registers => _registers.Value;
    }
}
