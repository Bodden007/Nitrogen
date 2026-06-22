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
                        Pressure_1 = pressure.HasError
                            ? pressure.ErrorText ?? "Ошибка"
                            : pressure.Value.ToString("F0");
                    }

                    if (values.TryGetValue("Opko_1", out var opko))
                    {
                        Opko_1 = opko.Value.ToString("F0");
                    }

                    if (values.TryGetValue("TemperatureOutlet", out var temperatureOutlet))
                    {
                        TemperatureOutlet = temperatureOutlet.HasError
                            ? temperatureOutlet.ErrorText ?? "Ошибка"
                            : temperatureOutlet.Value.ToString("F0");
                    }

                    if (values.TryGetValue("TemperatureVaporizer", out var temperatureVaporizer))
                    {
                        TemperatureVaporizer = temperatureVaporizer.HasError
                            ? temperatureVaporizer.ErrorText ?? "Ошибка"
                            : temperatureVaporizer.Value.ToString("F0");
                    }

                    if (values.TryGetValue("TemperatureBath", out var temperatureBath))
                    {
                        TemperatureBath = temperatureBath.HasError
                            ? temperatureBath.ErrorText ?? "Ошибка"
                            : temperatureBath.Value.ToString("F0");
                    }

                    if (values.TryGetValue("PUMP_1RPM", out var pump1Rpm))
                    {
                        Pump1Rpm = pump1Rpm.Value;
                    }

                    if (values.TryGetValue("PUMP_1SCFM", out var pump1Scfm))
                    {
                        Pump1Scfm = pump1Scfm.Value;
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

        private string _temperatureOutlet = "---";
        public string TemperatureOutlet
        {
            get => _temperatureOutlet;
            private set => this.RaiseAndSetIfChanged(ref _temperatureOutlet, value);
        }

        private string _temperatureVaporizer = "---";
        public string TemperatureVaporizer
        {
            get => _temperatureVaporizer;
            private set => this.RaiseAndSetIfChanged(ref _temperatureVaporizer, value);
        }

        private string _temperatureBath = "---";
        public string TemperatureBath
        {
            get => _temperatureBath;
            private set => this.RaiseAndSetIfChanged(ref _temperatureBath, value);
        }

        private double _pump1Rpm;
        public double Pump1Rpm
        {
            get => _pump1Rpm;
            private set => this.RaiseAndSetIfChanged(ref _pump1Rpm, value);
        }

        private double _pump1Scfm;
        public double Pump1Scfm
        {
            get => _pump1Scfm;
            private set => this.RaiseAndSetIfChanged(ref _pump1Scfm, value);
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