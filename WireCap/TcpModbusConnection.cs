using System.Net.Sockets;
using NModbus;

namespace WireCap;

public sealed class TcpModbusConnection : IModbusConnection
{
    private readonly TcpClient _tcpClient;

    public TcpModbusConnection(TcpClient tcpClient, IModbusMaster master)
    {
        _tcpClient = tcpClient;
        Master = master;
    }

    public IModbusMaster Master { get; }

    public void Dispose() => _tcpClient.Dispose();
}

public sealed class TcpModbusConnectionFactory : IModbusConnectionFactory
{
    public async Task<IModbusConnection> ConnectAsync(string host, int port)
    {
        var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(host, port);
        var master = new ModbusFactory().CreateMaster(tcpClient);
        return new TcpModbusConnection(tcpClient, master);
    }
}
