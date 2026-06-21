using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using Nitrogen.Services.Modbus.Connection;
using Nitrogen.Views.MainWindow;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Nitrogen.Views.Settings.SCFSet
{
    internal class SCFSetControlViewModel : ReactiveObject
    {
        private readonly MainWindowViewModel _mainVm;
        private readonly IModbusReader _reader;
        private readonly IModbusWriter _writer;
        private readonly IReadOnlyList<ModbusRegisterConfig> _holdingRegisters;
        private readonly ModbusConnectionConfig _connectionConfig;
        private readonly IReadOnlyList<ModbusRegisterConfig> _inputRegisters;

        public SCFSetControlViewModel(
            MainWindowViewModel mainVm,
            IModbusReader reader,
            IModbusWriter writer,
            ModbusConnectionConfig connectionConfig,
            IReadOnlyList<ModbusRegisterConfig> inputRegisters,
            IReadOnlyList<ModbusRegisterConfig> holdingRegisters)
        {
            _mainVm = mainVm;
            _reader = reader;
            _writer = writer;
            _connectionConfig = connectionConfig;
            _inputRegisters = inputRegisters;
            _holdingRegisters = holdingRegisters;

            _mainVm.PropertyChanged += MainVmOnPropertyChanged;
        }

        private void MainVmOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
