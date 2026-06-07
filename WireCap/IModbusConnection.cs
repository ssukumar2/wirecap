using NModbus;

namespace WireCap;

/// <summary>An open Modbus connection exposing a master. Dispose closes the transport.</summary>
public interface IModbusConnection : IDisposable
{
    IModbusMaster Master { get; }
}

/// <summary>Creates connected Modbus masters. Injectable so the client can be unit-tested.</summary>
public interface IModbusConnectionFactory
{
    Task<IModbusConnection> ConnectAsync(string host, int port);
}
