using Nitrogen.Services.Modbus.Configuration.Models.Registers;
namespace Nitrogen.Services.Modbus.Configuration.Models.Connection;

internal sealed class ModbusConnectionConfig
{
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    public byte SlaveId { get; set; }

    public int PollIntervalMs { get; set; }

    public ushort InputStartAddress { get; set; }

    public ushort InputRegisterCount { get; set; }

    public int ConnectTimeoutMs { get; set; }

    public int RequestTimeoutMs { get; set; }

    public int ReconnectDelayMs { get; set; }

    public FloatByteOrder FloatByteOrder { get; set; } = FloatByteOrder.ABCD;
}