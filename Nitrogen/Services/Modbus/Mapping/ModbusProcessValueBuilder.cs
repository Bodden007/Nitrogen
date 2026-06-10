using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using NModbus.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Nitrogen.Services.Modbus.Mapping;

internal sealed class ModbusProcessValueBuilder
{
    private readonly int _pressureLoIndex;
    private readonly int _pressureHiIndex;

    public ModbusProcessValueBuilder(
        IReadOnlyList<ModbusRegisterConfig> registersConfig,
        int inputStartAddress)
    {
        _pressureLoIndex = ToIndex(
            GetRegister(registersConfig, "Pressure_1Lo").Address,
            inputStartAddress);

        _pressureHiIndex = ToIndex(
            GetRegister(registersConfig, "Pressure_1Hi").Address,
            inputStartAddress);
    }

    public IReadOnlyDictionary<string, ProcessValue> Build(ushort[] registers)
    {
        var result = new Dictionary<string, ProcessValue>();

        float pressure = ModbusUtility.GetSingle(
            registers[_pressureHiIndex],
            registers[_pressureLoIndex]);

        result["Pressure_1"] = new ProcessValue
        {
            Name = "Pressure_1",
            Value = pressure,
            HasError = pressure == 22222,
            ErrorText = pressure == 22222 ? "Ошибка" : null
        };

        return result;
    }

    private static ModbusRegisterConfig GetRegister(
        IReadOnlyList<ModbusRegisterConfig> registersConfig,
        string name)
    {
        return registersConfig.First(x => x.Name == name);
    }

    private static int ToIndex(int address, int inputStartAddress)
    {
        return address - inputStartAddress;
    }
}