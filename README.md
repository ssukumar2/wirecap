![.NET Build](https://github.com/ssukumar2/wirecap/actions/workflows/dotnet.yml/badge.svg)

# wirecap

A Modbus TCP client for polling industrial devices, written in .NET 8 and C#. Reads holding registers from PLCs, solar inverters, energy meters, and other Modbus-speaking hardware. Supports plain text and structured JSON output.

## Why this project

Modbus TCP is the dominant protocol in industrial automation and energy infrastructure. Solar inverters, battery controllers, PLCs, and smart meters all speak it. wirecap is a small, portable CLI that reads from Modbus devices with clean output, making it easy to script, integrate, or use interactively.

## Features

- Read holding registers from any Modbus TCP device
- Configurable host, port, unit ID, start address, and register count
- Plain text or JSON output
- Async I/O throughout
- Structured logging

## Usage

Build:

    cd WireCap
    dotnet build

Read registers:

    dotnet run -- read --host 192.168.1.100 --port 502 --unit 1 --start 0 --count 10

JSON output:

    dotnet run -- read --host 192.168.1.100 --count 20 --json

## Stack

- .NET 8
- NModbus for Modbus TCP
- System.CommandLine for CLI parsing
- Microsoft.Extensions.Logging for structured logs

## Roadmap

- Write support (single and multiple registers)
- Read coils and discrete inputs
- Continuous polling mode with configurable interval
- Device profiles (named register maps for known inverters/meters)
- Modbus RTU support (serial)

## License

MIT