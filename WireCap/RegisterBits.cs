namespace WireCap;

/// <summary>Reads individual bits out of a 16-bit register (status/alarm words).</summary>
public static class RegisterBits
{
    public static bool IsSet(ushort register, int bit)
    {
        if (bit < 0 || bit > 15)
            throw new ArgumentOutOfRangeException(nameof(bit), "Bit index must be 0-15.");
        return (register & (1 << bit)) != 0;
    }

    public static bool[] ToBits(ushort register)
    {
        var bits = new bool[16];
        for (int i = 0; i < 16; i++)
            bits[i] = (register & (1 << i)) != 0;
        return bits;
    }
}
