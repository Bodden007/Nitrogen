using Microsoft.Win32;
using Nitrogen.Services.Modbus.Mapping;
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

        private readonly ModbusProcessValueBuilder _builder;

        public MainWindowViewModel(ISystemTimeService systemTimeService,
                                    IModbusRxService modbusRxService,
                                    ModbusProcessValueBuilder builder)
        {
            _builder = builder;
            _currentTime = systemTimeService.Ticks
                .ToProperty(this, vm => vm.CurrentTime, initialValue: DateTime.MinValue);

            var registersStream = modbusRxService.Registers
                .ObserveOn(RxSchedulers.MainThreadScheduler);

            _registers = registersStream
                .ToProperty(this, vm => vm.Registers, initialValue: new ushort[50]);

            //FIXME Диагностика Rx
            registersStream
                .Subscribe(registers =>
                {
                    var values = _builder.Build(registers);

                    if (values.TryGetValue("Pressure_1", out var pressure))
                    {
                        if (pressure.HasError)
                        {
                            Console.WriteLine($"{pressure.Name} = {pressure.ErrorText}");
                        }
                        else
                        {
                            Console.WriteLine($"{pressure.Name} = {pressure.Value:F0}");
                        }
                    }
                });

            modbusRxService.Start();


        }
        public DateTime CurrentTime => _currentTime.Value;

        public ushort[] Registers => _registers.Value;
    }
}
