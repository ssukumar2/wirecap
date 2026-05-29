namespace WireCap;

/// <summary>Linear conversion between raw register values and engineering units: physical = raw * Gain + Offset.</summary>
public sealed class RegisterScaler
{
    public double Gain { get; init; } = 1.0;
    public double Offset { get; init; }
    public string Unit { get; init; } = "";

    public double ToPhysical(double raw) => raw * Gain + Offset;

    public double ToPhysical(ushort register) => ToPhysical((double)register);

    public double FromPhysical(double physical) => Gain == 0 ? 0 : (physical - Offset) / Gain;
}
