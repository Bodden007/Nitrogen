using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using NModbus.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Nitrogen.Services.Modbus.Mapping;

internal sealed class ModbusProcessValueBuilder
{
    private readonly int _pressureLoIndex;
    private readonly int _pressureHiIndex;
    private readonly int _opkoLoIndex;
    private readonly int _opkoHiIndex;

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

        _opkoLoIndex = ToIndex(
            GetRegister(registersConfig, "Opko_1Lo").Address,
            inputStartAddress);

        _opkoHiIndex = ToIndex(
            GetRegister(registersConfig, "Opko_1Hi").Address,
            inputStartAddress);
    }

    public IReadOnlyDictionary<string, ProcessValue> Build(ushort[] registers)
    {
        var result = new Dictionary<string, ProcessValue>();

        float pressure = ModbusUtility.GetSingle(
            registers[_pressureHiIndex],
            registers[_pressureLoIndex]);

        float opko = ModbusUtility.GetSingle(
            registers[_opkoHiIndex],
            registers[_opkoLoIndex]);

        result["Pressure_1"] = new ProcessValue
        {
            Name = "Pressure_1",
            Value = pressure,
            HasError = pressure == 22222,
            ErrorText = pressure == 22222 ? "Ошибка" : null
        };

        result["Opko_1"] = new ProcessValue
        {
            Name = "Opko_1",
            Value = opko
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