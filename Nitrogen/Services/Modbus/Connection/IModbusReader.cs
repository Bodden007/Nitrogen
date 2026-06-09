using System.Threading;
using System.Threading.Tasks;

namespace Nitrogen.Services.Modbus.Connection;

internal interface IModbusReader
{
    Task<ushort[]> ReadInputRegistersAsync(
        byte slaveId,
        ushort startAddress,
        ushort count,
        CancellationToken cancellationToken = default);
}