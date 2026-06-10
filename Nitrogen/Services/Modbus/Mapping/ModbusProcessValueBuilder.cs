using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using NModbus.Utility;
using System.Collections.Generic;

namespace Nitrogen.Services.Modbus.Mapping;

internal sealed class ModbusProcessValueBuilder
{
    private readonly IReadOnlyList<ModbusRegisterConfig> _registersConfig;
    private readonly int _inputStartAddress;
    public ModbusProcessValueBuilder(
    IReadOnlyList<ModbusRegisterConfig> registersConfig,
    int inputStartAddress)
    {
        _registersConfig = registersConfig;
        _inputStartAddress = inputStartAddress;
    }
    public IReadOnlyDictionary<string, ProcessValue> Build(ushort[] registers)
    {
        var result = new Dictionary<string, ProcessValue>();

        if (registers.Length < 4)
            return result;

        float pressure = ModbusUtility.GetSingle
            (
                registers[3], // High
                registers[2]  // Low
            );

        result["Pressure_1"] = new ProcessValue
        {
            Name = "Pressure_1",
            Value = pressure,
            HasError = pressure == 22222,
            ErrorText = pressure == 22222 ? "Ошибка" : null
        };

        return result;
    }
}