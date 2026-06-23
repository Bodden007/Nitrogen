using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using Nitrogen.Services.Modbus.Connection;
using Nitrogen.Services.Modbus.Mapping.RegisterHelper;
using NModbus.Utility;
using ReactiveUI;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Nitrogen.Views.Menu.SCFVolume;

internal sealed class ScfVolumeControlViewModel : ReactiveObject
{
    private readonly IModbusReader _reader;
    private readonly ModbusConnectionConfig _connectionConfig;
    private readonly IReadOnlyList<ModbusRegisterConfig> _inputRegisters;

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

    public ScfVolumeControlViewModel(
        IModbusReader reader,
        ModbusConnectionConfig connectionConfig,
        IReadOnlyList<ModbusRegisterConfig> inputRegisters)
    {
        _reader = reader;
        _connectionConfig = connectionConfig;
        _inputRegisters = inputRegisters;
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
}