using Microsoft.Extensions.Logging;
using NModbus;

namespace WireCap;

public partial class ModbusClient
{
    public async Task<ushort[]> ReadInputRegistersAsync(
        byte unitId, ushort startAddress, ushort count)
    {
        _logger.LogInformation(
            "reading input registers {Host}:{Port} (unit={Unit}, start={Start}, count={Count})",
            _host, _port, unitId, startAddress, count);
        var values = await WithMasterAsync(m => m.ReadInputRegistersAsync(unitId, startAddress, count));
        _logger.LogInformation("read {Count} input registers successfully", values.Length);
        return values;
    }
}
