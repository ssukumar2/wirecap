using Microsoft.Extensions.Logging;
using NModbus;

namespace WireCap;

public partial class ModbusClient
{
    public async Task<bool[]> ReadCoilsAsync(
        byte unitId, ushort startAddress, ushort count)
    {
        _logger.LogInformation(
            "reading coils {Host}:{Port} (unit={Unit}, start={Start}, count={Count})",
            _host, _port, unitId, startAddress, count);
        var values = await WithMasterAsync(m => m.ReadCoilsAsync(unitId, startAddress, count));
        _logger.LogInformation("read {Count} coils successfully", values.Length);
        return values;
    }

    public async Task<bool[]> ReadDiscreteInputsAsync(
        byte unitId, ushort startAddress, ushort count)
    {
        _logger.LogInformation(
            "reading discrete inputs {Host}:{Port} (unit={Unit}, start={Start}, count={Count})",
            _host, _port, unitId, startAddress, count);
        var values = await WithMasterAsync(m => m.ReadInputsAsync(unitId, startAddress, count));
        _logger.LogInformation("read {Count} discrete inputs successfully", values.Length);
        return values;
    }
}
