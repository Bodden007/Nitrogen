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

namespace Nitrogen.Views.Settings.PressureSet;

internal sealed class PressureSetControlViewModel : ReactiveObject
{
    private readonly MainWindowViewModel _mainVm;
    private readonly IModbusReader _reader;
    private readonly IModbusWriter _writer;
    private readonly IReadOnlyList<ModbusRegisterConfig> _holdingRegisters;
    private readonly ModbusConnectionConfig _connectionConfig;
    private readonly IReadOnlyList<ModbusRegisterConfig> _inputRegisters;

    public PressureSetControlViewModel(
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

    public string Pressure_1 => _mainVm.Pressure_1;

    private string _pressure_1SrcMinEdit = "";
    public string Pressure_1SrcMinEdit
    {
        get => _pressure_1SrcMinEdit;
        set => this.RaiseAndSetIfChanged(ref _pressure_1SrcMinEdit, value);
    }

    private string _pressure_1SrcMaxEdit = "";
    public string Pressure_1SrcMaxEdit
    {
        get => _pressure_1SrcMaxEdit;
        set => this.RaiseAndSetIfChanged(ref _pressure_1SrcMaxEdit, value);
    }

    private string _pressure_1TargMinEdit = "";
    public string Pressure_1TargMinEdit
    {
        get => _pressure_1TargMinEdit;
        set => this.RaiseAndSetIfChanged(ref _pressure_1TargMinEdit, value);
    }

    private string _pressure_1TargMaxEdit = "";
    public string Pressure_1TargMaxEdit
    {
        get => _pressure_1TargMaxEdit;
        set => this.RaiseAndSetIfChanged(ref _pressure_1TargMaxEdit, value);
    }

    public async Task LoadAsync()
    {
        var startAddress = ModbusRegisterHelper.GetStartAddress(
            _inputRegisters,
            "Pressure_1SrcMinLo",
            "Pressure_1SrcMinHi",
            "Pressure_1SrcMaxLo",
            "Pressure_1SrcMaxHi",
            "Pressure_1TargMinLo",
            "Pressure_1TargMinHi",
            "Pressure_1TargMaxLo",
            "Pressure_1TargMaxHi");

        var registerCount = ModbusRegisterHelper.GetRegisterCount(
            _inputRegisters,
            "Pressure_1SrcMinLo",
            "Pressure_1SrcMinHi",
            "Pressure_1SrcMaxLo",
            "Pressure_1SrcMaxHi",
            "Pressure_1TargMinLo",
            "Pressure_1TargMinHi",
            "Pressure_1TargMaxLo",
            "Pressure_1TargMaxHi");

        Console.WriteLine($"PRESSURE SET LOAD: Start={startAddress}, Count={registerCount}");

        var registers = await _reader.ReadInputRegistersAsync(
            _connectionConfig.SlaveId,
            startAddress,
            registerCount);

        var srcMin = ModbusUtility.GetSingle(registers[1], registers[0]);
        var srcMax = ModbusUtility.GetSingle(registers[3], registers[2]);
        var targMin = ModbusUtility.GetSingle(registers[5], registers[4]);
        var targMax = ModbusUtility.GetSingle(registers[7], registers[6]);

        Pressure_1SrcMinEdit = srcMin.ToString("0.###", CultureInfo.InvariantCulture);
        Pressure_1SrcMaxEdit = srcMax.ToString("0.###", CultureInfo.InvariantCulture);
        Pressure_1TargMinEdit = targMin.ToString("0.###", CultureInfo.InvariantCulture);
        Pressure_1TargMaxEdit = targMax.ToString("0.###", CultureInfo.InvariantCulture);
    }

    public async Task SaveAsync()
    {
        var startAddress = ModbusRegisterHelper.GetStartAddress(
            _holdingRegisters,
            "Pressure_1SrcMinLo",
            "Pressure_1SrcMinHi",
            "Pressure_1SrcMaxLo",
            "Pressure_1SrcMaxHi",
            "Pressure_1TargMinLo",
            "Pressure_1TargMinHi",
            "Pressure_1TargMaxLo",
            "Pressure_1TargMaxHi");

        var srcMin = float.Parse(
            Pressure_1SrcMinEdit,
            CultureInfo.InvariantCulture);

        var srcMax = float.Parse(
            Pressure_1SrcMaxEdit,
            CultureInfo.InvariantCulture);

        var targMin = float.Parse(
            Pressure_1TargMinEdit,
            CultureInfo.InvariantCulture);

        var targMax = float.Parse(
            Pressure_1TargMaxEdit,
            CultureInfo.InvariantCulture);

        var registers = new ushort[8];

        WriteFloat(registers, 0, srcMin);
        WriteFloat(registers, 2, srcMax);
        WriteFloat(registers, 4, targMin);
        WriteFloat(registers, 6, targMax);

        await _writer.WriteMultipleRegistersAsync(
            _connectionConfig.SlaveId,
            (ushort)startAddress,
            registers);

        ushort setPrmAddress = ModbusRegisterHelper.GetStartAddress(
            _holdingRegisters,
            "Pressure_1SetPrmLo");

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
        if (e.PropertyName == nameof(MainWindowViewModel.Pressure_1))
            this.RaisePropertyChanged(nameof(Pressure_1));
    }
}