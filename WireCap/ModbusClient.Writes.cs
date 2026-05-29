using Microsoft.Extensions.Logging;
using NModbus;

namespace WireCap;

public partial class ModbusClient
{
    public async Task WriteMultipleRegistersAsync(
        byte unitId, ushort startAddress, ushort[] values)
    {
        _logger.LogInformation(
            "writing {Count} registers to {Host}:{Port} (unit={Unit}, start={Start})",
            values.Length, _host, _port, unitId, startAddress);
        await WithMasterAsync(m => m.WriteMultipleRegistersAsync(unitId, startAddress, values));
        _logger.LogInformation("wrote {Count} registers successfully", values.Length);
    }
}
