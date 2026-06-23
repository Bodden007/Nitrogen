using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using Nitrogen.Services.Modbus.Connection;
using Nitrogen.Services.Modbus.Mapping.RegisterHelper;
using Nitrogen.Views.MainWindow;
using NModbus.Utility;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;

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

        public double Pump1Rpm => _mainVm.Pump1Rpm;
        public double Pump1TotalScfm => _mainVm.Pump1TotalScfm;

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

        private string _pump_1TeethCountEdit = "";
        public string Pump_1TeethCountEdit
        {
            get => _pump_1TeethCountEdit;
            set => this.RaiseAndSetIfChanged(ref _pump_1TeethCountEdit, value);
        }

        private string _pump_1PumpFactorEdit = "";
        public string Pump_1PumpFactorEdit
        {
            get => _pump_1PumpFactorEdit;
            set => this.RaiseAndSetIfChanged(ref _pump_1PumpFactorEdit, value);
        }

        private string _pump_1PumpEfficiencyFactorEdit = "";
        public string Pump_1PumpEfficiencyFactorEdit
        {
            get => _pump_1PumpEfficiencyFactorEdit;
            set => this.RaiseAndSetIfChanged(ref _pump_1PumpEfficiencyFactorEdit, value);
        }

        public async Task LoadAsync()
        {
            var startAddress = ModbusRegisterHelper.GetStartAddress(
                _inputRegisters,
                "PUMP_1TeethCountLo",
                "PUMP_1PumpFactorLo",
                "PUMP_1PumpFactorHi",
                "PUMP_1PumpEfficiencyFactorLo",
                "PUMP_1PumpEfficiencyFactorHi");

            var registerCount = ModbusRegisterHelper.GetRegisterCount(
                _inputRegisters,
                "PUMP_1TeethCountLo",
                "PUMP_1PumpFactorLo",
                "PUMP_1PumpFactorHi",
                "PUMP_1PumpEfficiencyFactorLo",
                "PUMP_1PumpEfficiencyFactorHi");

            Console.WriteLine($"SCF SET LOAD: Start={startAddress}, Count={registerCount}");

            var registers = await _reader.ReadInputRegistersAsync(
                _connectionConfig.SlaveId,
                startAddress,
                registerCount);

            var teethCount = registers[0];
            var pumpFactor = ModbusUtility.GetSingle(registers[3], registers[2]);
            var efficiencyFactor = ModbusUtility.GetSingle(registers[5], registers[4]);

            Pump_1TeethCountEdit = teethCount.ToString(CultureInfo.InvariantCulture);
            Pump_1PumpFactorEdit = pumpFactor.ToString("0.###", CultureInfo.InvariantCulture);
            Pump_1PumpEfficiencyFactorEdit = efficiencyFactor.ToString("0.###", CultureInfo.InvariantCulture);
        }

        public async Task SaveAsync()
        {
            var startAddress = ModbusRegisterHelper.GetStartAddress(
                _holdingRegisters,
                "PUMP_1TeethCountLo",
                "PUMP_1PumpFactorLo",
                "PUMP_1PumpFactorHi",
                "PUMP_1PumpEfficiencyFactorLo",
                "PUMP_1PumpEfficiencyFactorHi");

            var teethCount = ushort.Parse(
                Pump_1TeethCountEdit,
                CultureInfo.InvariantCulture);

            var pumpFactor = float.Parse(
                Pump_1PumpFactorEdit,
                CultureInfo.InvariantCulture);

            var efficiencyFactor = float.Parse(
                Pump_1PumpEfficiencyFactorEdit,
                CultureInfo.InvariantCulture);

            var registers = new ushort[6];

            registers[0] = teethCount;
            // registers[1] = reserved address 121

            WriteFloat(registers, 2, pumpFactor);
            WriteFloat(registers, 4, efficiencyFactor);

            await _writer.WriteMultipleRegistersAsync(
                _connectionConfig.SlaveId,
                (ushort)startAddress,
                registers);

            ushort setPrmAddress = ModbusRegisterHelper.GetStartAddress(
                _holdingRegisters,
                "PUMP_1PumpSetPrmLo");

            await _writer.WriteSingleRegisterAsync(
                _connectionConfig.SlaveId,
                setPrmAddress,
                1);
        }

        private static void WriteFloat(ushort[] registers, int index, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            registers[index] = BitConverter.ToUInt16(bytes, 0);      // Lo
            registers[index + 1] = BitConverter.ToUInt16(bytes, 2);  // Hi
        }

        private void MainVmOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.Pump1Rpm))
                this.RaisePropertyChanged(nameof(Pump1Rpm));

            if (e.PropertyName == nameof(MainWindowViewModel.Pump1TotalScfm))
                this.RaisePropertyChanged(nameof(Pump1TotalScfm));
        }
    }
}
