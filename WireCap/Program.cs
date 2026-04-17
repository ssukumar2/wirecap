
using System.CommandLine;
using Microsoft.Extensions.Logging;
using WireCap;

var hostOption = new Option<string>(
    name: "--host",
    description: "Modbus TCP server hostname or IP",
    getDefaultValue: () => "localhost");

var portOption = new Option<int>(
    name: "--port",
    description: "Modbus TCP server port",
    getDefaultValue: () => 502);

var unitIdOption = new Option<byte>(
    name: "--unit",
    description: "Modbus slave unit ID",
    getDefaultValue: () => (byte)1);

var startAddressOption = new Option<ushort>(
    name: "--start",
    description: "Start address of holding registers to read",
    getDefaultValue: () => (ushort)0);

var countOption = new Option<ushort>(
    name: "--count",
    description: "Number of holding registers to read",
    getDefaultValue: () => (ushort)10);

var jsonOption = new Option<bool>(
    name: "--json",
    description: "Output as JSON",
    getDefaultValue: () => false);

var readCommand = new Command("read", "Read holding registers from a Modbus TCP device")
{
    hostOption, portOption, unitIdOption, startAddressOption, countOption, jsonOption,
};

readCommand.SetHandler(async (host, port, unit, start, count, json) =>
{
    using var loggerFactory = LoggerFactory.Create(builder =>
        builder.AddConsole().SetMinimumLevel(LogLevel.Information));
    var logger = loggerFactory.CreateLogger<Program>();

    var client = new ModbusClient(host, port, logger);
    try
    {
        var values = await client.ReadHoldingRegistersAsync(unit, start, count);
        var output = new RegisterReadResult(host, port, unit, start, values);

        if (json)
            Console.WriteLine(output.ToJson());
        else
            Console.WriteLine(output.ToText());
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "failed to read registers");
        Environment.Exit(1);
    }
}, hostOption, portOption, unitIdOption, startAddressOption, countOption, jsonOption);

var rootCommand = new RootCommand("wirecap — Modbus TCP client for industrial device polling")
{
    readCommand,
};

return await rootCommand.InvokeAsync(args);