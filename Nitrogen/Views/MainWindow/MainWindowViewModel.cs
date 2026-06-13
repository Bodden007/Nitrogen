using Avalonia.Media;
using Nitrogen.Services.Modbus.Mapping;
using Nitrogen.Services.Modbus.Rx;
using Nitrogen.Services.SystemTime;
using ReactiveUI;
using System;
using System.Collections.Generic;
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
                        //FIXME Диагностика Pressure_1 VM
                        Console.WriteLine(
                            $"VM Pressure_1 BEFORE={Pressure_1} RX_VALUE={pressure.Value} HasError={pressure.HasError}");

                        Pressure_1 = pressure.HasError
                            ? pressure.ErrorText ?? "Ошибка"
                            : pressure.Value.ToString("F0");

                        //FIXME Диагностика Pressure_1 VM
                        Console.WriteLine($"VM Pressure_1 AFTER={Pressure_1}");
                    }
                    else
                    {
                        //FIXME Диагностика Pressure_1 VM
                        Console.WriteLine("VM Pressure_1 NOT FOUND");
                    }

                    if (values.TryGetValue("Opko_1", out var opko))
                    {
                        Opko_1 = opko.Value.ToString("F0");
                    }

                    UpdateOpkoAlarm(values);
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

        private IBrush _pressureTileBackground = Brushes.Transparent;
        public IBrush PressureTileBackground
        {
            get => _pressureTileBackground;
            private set => this.RaiseAndSetIfChanged(ref _pressureTileBackground, value);
        }

        private IBrush _pressureHeaderBackground = Brushes.Transparent;
        public IBrush PressureHeaderBackground
        {
            get => _pressureHeaderBackground;
            private set => this.RaiseAndSetIfChanged(ref _pressureHeaderBackground, value);
        }

        private IBrush _pressureOpkoBackground = new SolidColorBrush(Color.Parse("#FBA9A3"));
        public IBrush PressureOpkoBackground
        {
            get => _pressureOpkoBackground;
            private set => this.RaiseAndSetIfChanged(ref _pressureOpkoBackground, value);
        }

        private void UpdateOpkoAlarm(IReadOnlyDictionary<string, ProcessValue> values)
        {
            var isAlarm =
                values.TryGetValue("StatOpko_1", out var statOpko)
                && !statOpko.HasError
                && statOpko.Value == 1;

            PressureTileBackground = isAlarm
                ? Brushes.Red
                : Brushes.Transparent;
        }
    }
}