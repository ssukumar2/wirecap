namespace WireCap;

/// <summary>Word ordering when combining two 16-bit registers into a 32-bit value.</summary>
public enum WordOrder
{
    HighWordFirst,
    LowWordFirst,
}

/// <summary>Decodes raw Modbus register arrays into numeric types. Pure functions, no I/O.</summary>
public static class RegisterDecoder
{
    public static short ToInt16(ushort register) => unchecked((short)register);

    public static ushort ToUInt16(ushort register) => register;

    public static uint ToUInt32(ushort[] registers, int offset = 0, WordOrder order = WordOrder.HighWordFirst)
    {
        ArgumentNullException.ThrowIfNull(registers);
        if (offset < 0 || offset + 2 > registers.Length)
            throw new ArgumentOutOfRangeException(nameof(offset), "Need 2 registers for a 32-bit value.");

        ushort hi = order == WordOrder.HighWordFirst ? registers[offset]     : registers[offset + 1];
        ushort lo = order == WordOrder.HighWordFirst ? registers[offset + 1] : registers[offset];
        return ((uint)hi << 16) | lo;
    }

    public static int ToInt32(ushort[] registers, int offset = 0, WordOrder order = WordOrder.HighWordFirst)
        => unchecked((int)ToUInt32(registers, offset, order));

    public static float ToFloat(ushort[] registers, int offset = 0, WordOrder order = WordOrder.HighWordFirst)
        => BitConverter.Int32BitsToSingle(ToInt32(registers, offset, order));
}
