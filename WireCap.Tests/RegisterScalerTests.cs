using WireCap;
using Xunit;

public class RegisterScalerTests
{
    [Fact]
    public void ToPhysical_applies_gain_and_offset()
    {
        var s = new RegisterScaler { Gain = 0.1, Offset = -40, Unit = "C" };
        Assert.Equal(20.0, s.ToPhysical((ushort)600), 9);
    }

    [Fact]
    public void FromPhysical_is_inverse()
    {
        var s = new RegisterScaler { Gain = 0.1, Offset = -40 };
        Assert.Equal(600.0, s.FromPhysical(20.0), 9);
    }

    [Fact]
    public void FromPhysical_with_zero_gain_returns_zero()
    {
        var s = new RegisterScaler { Gain = 0 };
        Assert.Equal(0.0, s.FromPhysical(123.0));
    }

    [Fact]
    public void Unit_defaults_to_empty() => Assert.Equal("", new RegisterScaler().Unit);
}
