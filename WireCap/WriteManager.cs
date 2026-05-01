using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace WireCap
{
    public class WriteManager
    {
        private readonly ModbusClient _client;
        private readonly ILogger _logger;
        private readonly int _maxRetries;
        private readonly int _retryDelayMs;

        public WriteManager(ModbusClient client, int maxRetries = 3,
                            int retryDelayMs = 500, ILogger logger = null)
        {
            _client = client;
            _maxRetries = maxRetries;
            _retryDelayMs = retryDelayMs;
            _logger = logger;
        }

        public bool WriteAndVerify(ushort address, ushort value)
        {
            for (int attempt = 1; attempt <= _maxRetries; attempt++)
            {
                try
                {
                    _client.WriteSingleRegister(address, value);
                    Thread.Sleep(100);

                    var readback = _client.ReadHoldingRegisters(address, 1);
                    if (readback != null && readback.Registers.Length > 0
                        && readback.Registers[0] == value)
                    {
                        _logger?.LogInformation(
                            "Write verified: addr={Address} value={Value} attempt={Attempt}",
                            address, value, attempt);
                        return true;
                    }

                    _logger?.LogWarning(
                        "Write verify mismatch: addr={Address} expected={Expected} got={Got}",
                        address, value, readback?.Registers[0]);
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(
                        "Write attempt {Attempt} failed: {Error}", attempt, ex.Message);
                }

                if (attempt < _maxRetries)
                    Thread.Sleep(_retryDelayMs);
            }

            _logger?.LogError("Write failed after {MaxRetries} attempts: addr={Address}",
                              _maxRetries, address);
            return false;
        }

        public bool WriteMultipleAndVerify(ushort startAddress, ushort[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (!WriteAndVerify((ushort)(startAddress + i), values[i]))
                    return false;
            }
            return true;
        }
    }
}
