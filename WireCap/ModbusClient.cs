using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using NModbus;

namespace WireCap;

public partial class ModbusClient
{
    private readonly string _host;
    private readonly int _port;
    private readonly ILogger _logger;

    public ModbusClient(string host, int port, ILogger logger)
    {
        _host = host;
        _port = port;
        _logger = logger;
    }

    private async Task<T> WithMasterAsync<T>(Func<IModbusMaster, Task<T>> action)
    {
        using var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(_host, _port);
        var master = new ModbusFactory().CreateMaster(tcpClient);
        return await action(master);
    }

    private async Task WithMasterAsync(Func<IModbusMaster, Task> action)
    {
        using var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(_host, _port);
        var master = new ModbusFactory().CreateMaster(tcpClient);
        await action(master);
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
