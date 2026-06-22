using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using NModbus.Utility;
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
    private readonly int _temperatureOutletLoIndex;
    private readonly int _temperatureOutletHiIndex;
    private readonly int _temperatureVaporizerLoIndex;
    private readonly int _temperatureVaporizerHiIndex;
    private readonly int _temperatureBathLoIndex;
    private readonly int _temperatureBathHiIndex;
    private readonly int _pump1RpmLoIndex;
    private readonly int _pump1RpmHiIndex;
    private readonly int _pump1ScfmLoIndex;
    private readonly int _pump1ScfmHiIndex;

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

        _temperatureOutletLoIndex = ToIndex(
            GetRegister(registersConfig, "TemperatureOutletLo").Address,
            inputStartAddress);

        _temperatureOutletHiIndex = ToIndex(
            GetRegister(registersConfig, "TemperatureOutletHi").Address,
            inputStartAddress);

        _temperatureVaporizerLoIndex = ToIndex(
            GetRegister(registersConfig, "TemperatureVaporizerLo").Address,
            inputStartAddress);

        _temperatureVaporizerHiIndex = ToIndex(
            GetRegister(registersConfig, "TemperatureVaporizerHi").Address,
            inputStartAddress);

        _temperatureBathLoIndex = ToIndex(
            GetRegister(registersConfig, "TemperatureBathLo").Address,
            inputStartAddress);

        _temperatureBathHiIndex = ToIndex(
            GetRegister(registersConfig, "TemperatureBathHi").Address,
            inputStartAddress);

        _pump1RpmLoIndex = ToIndex(
            GetRegister(registersConfig, "PUMP_1RPMLo").Address,
            inputStartAddress);

        _pump1RpmHiIndex = ToIndex(
            GetRegister(registersConfig, "PUMP_1RPMHi").Address,
            inputStartAddress);

        _pump1ScfmLoIndex = ToIndex(
            GetRegister(registersConfig, "PUMP_1SCFMLo").Address,
            inputStartAddress);

        _pump1ScfmHiIndex = ToIndex(
            GetRegister(registersConfig, "PUMP_1SCFMHi").Address,
            inputStartAddress);
    }

    public IReadOnlyDictionary<string, ProcessValue> Build(ushort[] registers)
    {
        var result = new Dictionary<string, ProcessValue>();

        if (registers.Length <= _pressureLoIndex ||
            registers.Length <= _pressureHiIndex ||
            registers.Length <= _opkoLoIndex ||
            registers.Length <= _opkoHiIndex ||
            registers.Length <= _statOpkoIndex ||
            registers.Length <= _temperatureOutletLoIndex ||
            registers.Length <= _temperatureOutletHiIndex ||
            registers.Length <= _temperatureVaporizerLoIndex ||
            registers.Length <= _temperatureVaporizerHiIndex ||
            registers.Length <= _temperatureBathLoIndex ||
            registers.Length <= _temperatureBathHiIndex ||
            registers.Length <= _pump1RpmLoIndex ||
            registers.Length <= _pump1RpmHiIndex ||
            registers.Length <= _pump1ScfmLoIndex ||
            registers.Length <= _pump1ScfmHiIndex
            )
        {
            return result;
        }

        float pressure = ModbusUtility.GetSingle(
            registers[_pressureHiIndex],
            registers[_pressureLoIndex]);

        float opko = ModbusUtility.GetSingle(
            registers[_opkoHiIndex],
            registers[_opkoLoIndex]);

        float temperatureOutlet = ModbusUtility.GetSingle(
            registers[_temperatureOutletHiIndex],
            registers[_temperatureOutletLoIndex]);

        float temperatureVaporizer = ModbusUtility.GetSingle(
            registers[_temperatureVaporizerHiIndex],
            registers[_temperatureVaporizerLoIndex]);

        float temperatureBath = ModbusUtility.GetSingle(
            registers[_temperatureBathHiIndex],
            registers[_temperatureBathLoIndex]);

        float pump1Rpm = ModbusUtility.GetSingle(
            registers[_pump1RpmHiIndex],
            registers[_pump1RpmLoIndex]);

        float pump1Scfm = ModbusUtility.GetSingle(
            registers[_pump1ScfmHiIndex],
            registers[_pump1ScfmLoIndex]);

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

        result["TemperatureOutlet"] = new ProcessValue
        {
            Name = "TemperatureOutlet",
            Value = temperatureOutlet,
            HasError = temperatureOutlet == 22222,
            ErrorText = temperatureOutlet == 22222 ? "Ошибка" : null
        };

        result["TemperatureVaporizer"] = new ProcessValue
        {
            Name = "TemperatureVaporizer",
            Value = temperatureVaporizer,
            HasError = temperatureVaporizer == 22222,
            ErrorText = temperatureVaporizer == 22222 ? "Ошибка" : null
        };

        result["TemperatureBath"] = new ProcessValue
        {
            Name = "TemperatureBath",
            Value = temperatureBath,
            HasError = temperatureBath == 22222,
            ErrorText = temperatureBath == 22222 ? "Ошибка" : null
        };
        result["PUMP_1RPM"] = new ProcessValue
        {
            Name = "PUMP_1RPM",
            Value = pump1Rpm
        };

        result["PUMP_1SCFM"] = new ProcessValue
        {
            Name = "PUMP_1SCFM",
            Value = pump1Scfm
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