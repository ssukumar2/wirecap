using Microsoft.Extensions.Logging;
using NModbus;

namespace WireCap;

public partial class ModbusClient
{
    private readonly string _host;
    private readonly int _port;
    private readonly ILogger _logger;
    private readonly IModbusConnectionFactory _connectionFactory;

    public ModbusClient(string host, int port, ILogger logger, IModbusConnectionFactory? connectionFactory = null)
    {
        _host = host;
        _port = port;
        _logger = logger;
        _connectionFactory = connectionFactory ?? new TcpModbusConnectionFactory();
    }

    private async Task<T> WithMasterAsync<T>(Func<IModbusMaster, Task<T>> action)
    {
        using var connection = await _connectionFactory.ConnectAsync(_host, _port);
        return await action(connection.Master);
    }

    private async Task WithMasterAsync(Func<IModbusMaster, Task> action)
    {
        using var connection = await _connectionFactory.ConnectAsync(_host, _port);
        await action(connection.Master);
    }

    public async Task<ushort[]> ReadHoldingRegistersAsync(
        byte unitId, ushort startAddress, ushort count)
    {
        _logger.LogInformation(
            "connecting to {Host}:{Port} (unit={Unit}, start={Start}, count={Count})",
            _host, _port, unitId, startAddress, count);
        var values = await WithMasterAsync(m => m.ReadHoldingRegistersAsync(unitId, startAddress, count));
        _logger.LogInformation("read {Count} holding registers successfully", values.Length);
        return values;
    }

    public async Task WriteSingleRegisterAsync(
        byte unitId, ushort address, ushort value)
    {
        _logger.LogInformation(
            "connecting to {Host}:{Port} (unit={Unit}, writing {Value} to address {Address})",
            _host, _port, unitId, value, address);
        await WithMasterAsync(m => m.WriteSingleRegisterAsync(unitId, address, value));
        _logger.LogInformation("wrote register {Address} = {Value} successfully", address, value);
    }
}
