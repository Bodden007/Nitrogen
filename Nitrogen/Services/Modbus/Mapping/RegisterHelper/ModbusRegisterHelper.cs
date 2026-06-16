using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using System.Collections.Generic;
using System.Linq;

namespace Nitrogen.Services.Modbus.Mapping.RegisterHelper;

internal static class ModbusRegisterHelper
{
    public static ModbusRegisterConfig GetRegister(
        IReadOnlyList<ModbusRegisterConfig> registers,
        string name)
    {
        return registers.First(x => x.Name == name);
    }

    public static ushort GetStartAddress(
        IReadOnlyList<ModbusRegisterConfig> registers,
        params string[] names)
    {
        return (ushort)names
            .Select(name => GetRegister(registers, name).Address)
            .Min();
    }

    public static ushort GetRegisterCount(
        IReadOnlyList<ModbusRegisterConfig> registers,
        params string[] names)
    {
        int minAddress = names
            .Select(name => GetRegister(registers, name).Address)
            .Min();

        int maxAddress = names
            .Select(name => GetRegister(registers, name).Address)
            .Max();

        return (ushort)(maxAddress - minAddress + 1);
    }
}