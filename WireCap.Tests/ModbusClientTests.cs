using Microsoft.Extensions.Logging.Abstractions;
using NModbus;
using NSubstitute;
using WireCap;
using Xunit;

public class ModbusClientTests
{
    private static (ModbusClient client, IModbusMaster master) CreateClient()
    {
        var master = Substitute.For<IModbusMaster>();
        var connection = Substitute.For<IModbusConnection>();
        connection.Master.Returns(master);
        var factory = Substitute.For<IModbusConnectionFactory>();
        factory.ConnectAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(connection);
        return (new ModbusClient("localhost", 502, NullLogger.Instance, factory), master);
    }

    [Fact]
    public async Task ReadHoldingRegistersAsync_returns_master_values()
    {
        var (client, master) = CreateClient();
        master.ReadHoldingRegistersAsync(1, 0, 2).Returns(new ushort[] { 10, 20 });
        Assert.Equal(new ushort[] { 10, 20 }, await client.ReadHoldingRegistersAsync(1, 0, 2));
    }

    [Fact]
    public async Task ReadCoilsAsync_returns_master_values()
    {
        var (client, master) = CreateClient();
        master.ReadCoilsAsync(1, 0, 3).Returns(new[] { true, false, true });
        Assert.Equal(new[] { true, false, true }, await client.ReadCoilsAsync(1, 0, 3));
    }

    [Fact]
    public async Task WriteSingleRegisterAsync_forwards_to_master()
    {
        var (client, master) = CreateClient();
        await client.WriteSingleRegisterAsync(1, 5, 99);
        await master.Received(1).WriteSingleRegisterAsync(1, 5, 99);
    }

    [Fact]
    public async Task WriteMultipleRegistersAsync_forwards_to_master()
    {
        var (client, master) = CreateClient();
        var values = new ushort[] { 1, 2, 3 };
        await client.WriteMultipleRegistersAsync(1, 10, values);
        await master.Received(1).WriteMultipleRegistersAsync(1, 10, values);
    }
}
