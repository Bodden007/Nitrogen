using System.Threading;
using System.Threading.Tasks;

namespace Nitrogen.Services.Modbus.Connection;

internal interface IModbusWriter
{
    Task WriteSingleRegisterAsync(
        byte slaveId,
        ushort address,
        ushort value,
        CancellationToken cancellationToken = default);

    Task WriteMultipleRegistersAsync(
        byte slaveId,
        ushort startAddress,
        ushort[] values,
        CancellationToken cancellationToken = default);
}