using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WireCap
{
    public class PollScheduler
    {
        private readonly ModbusClient _client;
        private readonly ILogger _logger;
        private readonly List<RegisterDefinition> _registers;
        private readonly int _intervalMs;
        private CancellationTokenSource _cts;

        public event Action<string, double, string> OnReading;

        public PollScheduler(ModbusClient client, List<RegisterDefinition> registers,
                             int intervalMs = 5000, ILogger logger = null)
        {
            _client = client;
            _registers = registers;
            _intervalMs = intervalMs;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            _cts = new CancellationTokenSource();
            _logger?.LogInformation("Poll scheduler started, interval={Interval}ms", _intervalMs);

            while (!_cts.Token.IsCancellationRequested)
            {
                foreach (var reg in _registers)
                {
                    try
                    {
                        var result = _client.ReadHoldingRegisters(reg.Address, (ushort)reg.RegisterCount);
                        if (result != null && result.Registers.Length > 0)
                        {
                            double value = result.Registers[0] * reg.ScaleFactor;
                            OnReading?.Invoke(reg.Name, value, reg.Unit);
                            _logger?.LogDebug("{Name}={Value}{Unit}", reg.Name, value, reg.Unit);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning("Failed to read {Name}: {Error}", reg.Name, ex.Message);
                    }
                }

                try { await Task.Delay(_intervalMs, _cts.Token); }
                catch (TaskCanceledException) { break; }
            }
        }

        public void Stop()
        {
            _cts?.Cancel();
            _logger?.LogInformation("Poll scheduler stopped");
        }
    }
}
