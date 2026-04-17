using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using NModbus;

namespace WireCap;

public class ModbusClient
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

    public async Task<ushort[]> ReadHoldingRegistersAsync(
        byte unitId, ushort startAddress, ushort count)
    {
        _logger.LogInformation(
            "connecting to {Host}:{Port} (unit={Unit}, start={Start}, count={Count})",
            _host, _port, unitId, startAddress, count);

        using var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(_host, _port);

        var factory = new ModbusFactory();
        var master = factory.CreateMaster(tcpClient);

        var values = await master.ReadHoldingRegistersAsync(unitId, startAddress, count);

        _logger.LogInformation("read {Count} registers successfully", values.Length);
        return values;
    }
}