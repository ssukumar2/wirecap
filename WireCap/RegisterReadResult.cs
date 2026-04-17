using System.Text;
using System.Text.Json;

namespace WireCap;

public record RegisterReadResult(
    string Host,
    int Port,
    byte UnitId,
    ushort StartAddress,
    ushort[] Values)
{
    public string ToText()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Modbus read from {Host}:{Port}, unit {UnitId}");
        sb.AppendLine($"  registers {StartAddress}..{StartAddress + Values.Length - 1}");
        for (var i = 0; i < Values.Length; i++)
        {
            sb.AppendLine($"  [{StartAddress + i,5}] = 0x{Values[i]:X4}  ({Values[i]})");
        }
        return sb.ToString();
    }

    public string ToJson()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(new
        {
            host = Host,
            port = Port,
            unit_id = UnitId,
            start_address = StartAddress,
            values = Values,
        }, options);
    }
}