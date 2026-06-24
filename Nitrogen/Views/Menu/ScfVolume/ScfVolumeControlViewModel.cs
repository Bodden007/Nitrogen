using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using Nitrogen.Services.Modbus.Connection;
using Nitrogen.Services.Modbus.Mapping.RegisterHelper;
using NModbus.Utility;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace Nitrogen.Views.Menu.SCFVolume;

internal sealed class ScfVolumeControlViewModel : ReactiveObject
{
    private readonly IModbusReader _reader;
    private readonly IModbusWriter _writer;
    private readonly ModbusConnectionConfig _connectionConfig;
    private readonly IReadOnlyList<ModbusRegisterConfig> _inputRegisters;
    private readonly IReadOnlyList<ModbusRegisterConfig> _holdingRegisters;

    private string _totalScfStageEdit = string.Empty;
    public string TotalScfStageEdit
    {
        get => _totalScfStageEdit;
        set => this.RaiseAndSetIfChanged(ref _totalScfStageEdit, value);
    }

    private string _pump1ScfStageEdit = string.Empty;
    public string Pump1ScfStageEdit
    {
        get => _pump1ScfStageEdit;
        set => this.RaiseAndSetIfChanged(ref _pump1ScfStageEdit, value);
    }

    private string _totalScfJobEdit = string.Empty;
    public string TotalScfJobEdit
    {
        get => _totalScfJobEdit;
        set => this.RaiseAndSetIfChanged(ref _totalScfJobEdit, value);
    }

    private string _pump1ScfJobEdit = string.Empty;
    public string Pump1ScfJobEdit
    {
        get => _pump1ScfJobEdit;
        set => this.RaiseAndSetIfChanged(ref _pump1ScfJobEdit, value);
    }

    public ReactiveCommand<Unit, Unit> SetCommand { get; }
    public ReactiveCommand<Unit, Unit> ResetStageCommand { get; }
    public ReactiveCommand<Unit, Unit> ResetJobCommand { get; }

    public ScfVolumeControlViewModel(
        IModbusReader reader,
        IModbusWriter writer,
        ModbusConnectionConfig connectionConfig,
        IReadOnlyList<ModbusRegisterConfig> inputRegisters,
        IReadOnlyList<ModbusRegisterConfig> holdingRegisters)
    {
        _reader = reader;
        _writer = writer;
        _connectionConfig = connectionConfig;
        _inputRegisters = inputRegisters;
        _holdingRegisters = holdingRegisters;

        SetCommand = ReactiveCommand.CreateFromTask(SetAsync);
        ResetStageCommand = ReactiveCommand.CreateFromTask(ResetStageAsync);
        ResetJobCommand = ReactiveCommand.CreateFromTask(ResetJobAsync);
    }

    public async Task LoadAsync()
    {
        Pump1ScfStageEdit = await ReadRealAsync("PUMP_1SCFStageLo");
        TotalScfStageEdit = await ReadRealAsync("TotalSCFStageLo");
        Pump1ScfJobEdit = await ReadRealAsync("PUMP_1SCFJobLo");
        TotalScfJobEdit = await ReadRealAsync("TotalSCFJobLo");
    }

    private async Task<string> ReadRealAsync(string loRegisterName)
    {
        var startAddress = (ushort)ModbusRegisterHelper
            .GetRegister(_inputRegisters, loRegisterName)
            .Address;

        var registers = await _reader.ReadInputRegistersAsync(
            _connectionConfig.SlaveId,
            startAddress,
            2);

        var value = ModbusUtility.GetSingle(registers[1], registers[0]);

        return value.ToString("0.###", CultureInfo.InvariantCulture);
    }

    private async Task SetAsync()
    {
        if (!TryParse(Pump1ScfStageEdit, out var pumpStage))
            return;

        if (!TryParse(Pump1ScfJobEdit, out var pumpJob))
            return;

        if (!TryParse(TotalScfStageEdit, out var totalStage))
            return;

        if (!TryParse(TotalScfJobEdit, out var totalJob))
            return;

        await _writer.WriteMultipleRegistersAsync(
            _connectionConfig.SlaveId,
            GetHoldingAddress("PUMP_1SCFStageLo"),
            FloatToRegisters(pumpStage));

        await _writer.WriteMultipleRegistersAsync(
            _connectionConfig.SlaveId,
            GetHoldingAddress("PUMP_1SCFJobLo"),
            FloatToRegisters(pumpJob));

        await _writer.WriteMultipleRegistersAsync(
            _connectionConfig.SlaveId,
            GetHoldingAddress("TotalSCFStageLo"),
            FloatToRegisters(totalStage));

        await _writer.WriteMultipleRegistersAsync(
            _connectionConfig.SlaveId,
            GetHoldingAddress("TotalSCFJobLo"),
            FloatToRegisters(totalJob));

        await _writer.WriteSingleRegisterAsync(
            _connectionConfig.SlaveId,
            GetHoldingAddress("TotalSCFCommandJobSetLo"),
            1);

        await Task.Delay(1000);

        await LoadAsync();   
    }

    private async Task ResetStageAsync()
    {
        ushort address = GetHoldingAddress("TotalSCFCommandStageResetLo");

        await _writer.WriteSingleRegisterAsync(
            _connectionConfig.SlaveId,
            address,
           1);

        await Task.Delay(1000);

        await LoadAsync();
    }

    private async Task ResetJobAsync()
    {
        ushort address = GetHoldingAddress("TotalSCFCommandJobResetLo");

        await _writer.WriteSingleRegisterAsync(
            _connectionConfig.SlaveId,
            address,
            1);

        await Task.Delay(1000);

        await LoadAsync();
    }

    private static bool TryParse(string text, out float value)
    {
        return float.TryParse(
            text.Replace(',', '.'),
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out value);
    }

    private static ushort[] FloatToRegisters(float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);

        ushort lo = BitConverter.ToUInt16(bytes, 0);
        ushort hi = BitConverter.ToUInt16(bytes, 2);

        return [lo, hi];
    }

    private ushort GetHoldingAddress(string name)
    {
        return (ushort)_holdingRegisters
            .First(x => x.Name == name)
            .Address;
    }
}