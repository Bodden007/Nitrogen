namespace Nitrogen.Services.Modbus.Configuration.Models.Registers;

internal sealed class ModbusRegisterConfig
{
    public string Name { get; set; } = string.Empty;

    public ushort Address { get; set; }

    public ModbusRegisterValueType Type { get; set; }
}