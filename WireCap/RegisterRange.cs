namespace WireCap;

/// <summary>A validated Modbus register range. A single read is limited to 125 registers.</summary>
public readonly struct RegisterRange
{
    public const int MaxRegistersPerRead = 125;

    public ushort Start { get; }
    public ushort Count { get; }

    public RegisterRange(ushort start, ushort count)
    {
        if (count == 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be at least 1.");
        if (count > MaxRegistersPerRead)
            throw new ArgumentOutOfRangeException(nameof(count), $"A single read is limited to {MaxRegistersPerRead} registers.");
        if (start + count - 1 > ushort.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(start), "Range exceeds the 16-bit address space.");

        Start = start;
        Count = count;
    }

    public ushort End => (ushort)(Start + Count - 1);
}
