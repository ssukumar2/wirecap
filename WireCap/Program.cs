
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

var writeValueOption = new Option<ushort>(
    name: "--value",
    description: "Value to write to the register");

var writeCommand = new Command("write", "Write a value to a holding register")
{
    hostOption, portOption, unitIdOption, startAddressOption, writeValueOption,
};

writeCommand.SetHandler(async (host, port, unit, start, value) =>
{
    using var loggerFactory = LoggerFactory.Create(builder =>
        builder.AddConsole().SetMinimumLevel(LogLevel.Information));
    var logger = loggerFactory.CreateLogger<Program>();

    var client = new ModbusClient(host, port, logger);
    try
    {
        await client.WriteSingleRegisterAsync(unit, start, value);
        Console.WriteLine($"wrote value {value} (0x{value:X4}) to register {start}");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "failed to write register");
        Environment.Exit(1);
    }
}, hostOption, portOption, unitIdOption, startAddressOption, writeValueOption);

var intervalOption = new Option<int>(
    name: "--interval",
    description: "Polling interval in seconds",
    getDefaultValue: () => 5);

var pollCommand = new Command("poll", "Continuously read holding registers at an interval")
{
    hostOption, portOption, unitIdOption, startAddressOption, countOption, jsonOption, intervalOption,
};

pollCommand.SetHandler(async (host, port, unit, start, count, json, interval) =>
{
    using var loggerFactory = LoggerFactory.Create(builder =>
        builder.AddConsole().SetMinimumLevel(LogLevel.Information));
    var logger = loggerFactory.CreateLogger<Program>();

    var client = new ModbusClient(host, port, logger);
    var pollCount = 0;

    Console.CancelKeyPress += (_, e) =>
    {
        e.Cancel = true;
        Console.WriteLine($"\nstopped after {pollCount} polls");
        Environment.Exit(0);
    };

    Console.WriteLine($"polling {host}:{port} every {interval}s (press Ctrl+C to stop)");

    while (true)
    {
        try
        {
            var values = await client.ReadHoldingRegistersAsync(unit, start, count);
            pollCount++;
            var timestamp = DateTime.Now.ToString("HH:mm:ss");

            if (json)
            {
                var output = new RegisterReadResult(host, port, unit, start, values);
                Console.WriteLine($"[{timestamp}] {output.ToJson()}");
            }
            else
            {
                var vals = string.Join(" ", values.Select(v => v.ToString()));
                Console.WriteLine($"[{timestamp}] registers {start}..{start + count - 1}: {vals}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] error: {ex.Message}");
        }

        await Task.Delay(interval * 1000);
    }
}, hostOption, portOption, unitIdOption, startAddressOption, countOption, jsonOption, intervalOption);


var rootCommand = new RootCommand("wirecap — Modbus TCP client for industrial device polling")
{
    readCommand,
    writeCommand,
    pollCommand,
};

return await rootCommand.InvokeAsync(args);