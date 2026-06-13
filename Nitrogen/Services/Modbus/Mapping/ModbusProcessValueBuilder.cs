using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using NModbus.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Nitrogen.Services.Modbus.Mapping;

internal sealed class ModbusProcessValueBuilder
{
    private readonly int _pressureLoIndex;
    private readonly int _pressureHiIndex;
    private readonly int _opkoLoIndex;
    private readonly int _opkoHiIndex;
    private readonly int _statOpkoIndex;

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

        _statOpkoIndex = ToIndex(
            GetRegister(registersConfig, "StatOpko_1").Address,
            inputStartAddress);
    }

    public IReadOnlyDictionary<string, ProcessValue> Build(ushort[] registers)
    {
        var result = new Dictionary<string, ProcessValue>();

        Console.WriteLine(
            $"BUILD ENTER len={registers.Length} " +
            $"pLoIdx={_pressureLoIndex} pHiIdx={_pressureHiIndex} " +
            $"opkoLoIdx={_opkoLoIndex} opkoHiIdx={_opkoHiIndex} statIdx={_statOpkoIndex}");

        if (registers.Length <= _pressureLoIndex ||
            registers.Length <= _pressureHiIndex ||
            registers.Length <= _opkoLoIndex ||
            registers.Length <= _opkoHiIndex ||
            registers.Length <= _statOpkoIndex)
        {
            Console.WriteLine("BUILD ERROR: index out of range");
            return result;
        }

        float pressure = ModbusUtility.GetSingle(
            registers[_pressureHiIndex],
            registers[_pressureLoIndex]);

        float opko = ModbusUtility.GetSingle(
            registers[_opkoHiIndex],
            registers[_opkoLoIndex]);

        Console.WriteLine(
            $"BUILD RAW Pressure Lo={registers[_pressureLoIndex]} Hi={registers[_pressureHiIndex]} Value={pressure}");

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

        result["StatOpko_1"] = new ProcessValue
        {
            Name = "StatOpko_1",
            Value = registers[_statOpkoIndex]
        };

        Console.WriteLine($"BUILD Pressure_1={pressure}");

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