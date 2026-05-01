using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Diagnostics;

namespace WireCap
{
    public static class Diagnostics
    {
        public static DiagnosticResult TestConnection(string host, int port, int timeoutMs = 3000)
        {
            var result = new DiagnosticResult { Host = host, Port = port };
            var sw = Stopwatch.StartNew();

            // TCP connection test
            try
            {
                using var client = new TcpClient();
                var task = client.ConnectAsync(host, port);
                if (task.Wait(timeoutMs))
                {
                    sw.Stop();
                    result.TcpConnected = true;
                    result.LatencyMs = sw.ElapsedMilliseconds;
                }
                else
                {
                    result.TcpConnected = false;
                    result.Error = "connection timeout";
                }
            }
            catch (Exception ex)
            {
                result.TcpConnected = false;
                result.Error = ex.InnerException?.Message ?? ex.Message;
            }

            // Ping test
            try
            {
                using var ping = new Ping();
                var reply = ping.Send(host, timeoutMs);
                result.PingReachable = reply.Status == IPStatus.Success;
                result.PingMs = reply.RoundtripTime;
            }
            catch
            {
                result.PingReachable = false;
            }

            return result;
        }

        public static void PrintResult(DiagnosticResult r)
        {
            Console.WriteLine($"=== Connection Diagnostics: {r.Host}:{r.Port} ===");
            Console.WriteLine($"  Ping:  {(r.PingReachable ? $"OK ({r.PingMs}ms)" : "FAILED")}");
            Console.WriteLine($"  TCP:   {(r.TcpConnected ? $"OK ({r.LatencyMs}ms)" : $"FAILED ({r.Error})")}");
        }
    }

    public class DiagnosticResult
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool PingReachable { get; set; }
        public long PingMs { get; set; }
        public bool TcpConnected { get; set; }
        public long LatencyMs { get; set; }
        public string Error { get; set; }
    }
}
