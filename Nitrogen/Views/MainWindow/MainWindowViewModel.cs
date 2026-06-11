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

        public MainWindowViewModel(
            ISystemTimeService timeService,
            IModbusRxService modbusRxService)
        {
            _currentTime = timeService.Ticks
                .ToProperty(this, vm => vm.CurrentTime, initialValue: DateTime.MinValue);

            var registersStream = modbusRxService.Registers
                 .ObserveOn(RxSchedulers.MainThreadScheduler);

            _registers = registersStream
                .ToProperty(this, vm => vm.Registers, initialValue: new ushort[50]);

            modbusRxService.ProcessValues
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Subscribe(values =>
                {
                    if (values.TryGetValue("Pressure_1", out var pressure))
                    {
                        Pressure_1 = pressure.HasError
                            ? pressure.ErrorText ?? "Ошибка"
                            : pressure.Value.ToString("F0");
                    }

                    if (values.TryGetValue("Opko_1", out var opko))
                    {
                        Opko_1 = opko.Value.ToString("F0");
                    }
                });

            modbusRxService.Start();
        }

        public DateTime CurrentTime => _currentTime.Value;

        public ushort[] Registers => _registers.Value;

        private string _pressure1 = "---";
        public string Pressure_1
        {
            get => _pressure1;
            private set => this.RaiseAndSetIfChanged(ref _pressure1, value);
        }

        private string _opko1 = "---";
        public string Opko_1
        {
            get => _opko1;
            private set => this.RaiseAndSetIfChanged(ref _opko1, value);
        }

        private string _opkoEdit = "---";
        public string OpkoEdit
        {
            get => _opkoEdit;
            set => this.RaiseAndSetIfChanged(ref _opkoEdit, value);
        }
        public void LoadOpkoEditOnce()
        {
            OpkoEdit = Opko_1;
        }
    }
}