using WireCap;
using Xunit;

public class RegisterBitsTests
{
    [Fact]
    public void IsSet_reads_low_bit() => Assert.True(RegisterBits.IsSet(0x0001, 0));

    [Fact]
    public void IsSet_reads_high_bit() => Assert.True(RegisterBits.IsSet(0x8000, 15));

    [Fact]
    public void IsSet_false_when_clear() => Assert.False(RegisterBits.IsSet(0x0001, 1));

    [Fact]
    public void IsSet_throws_out_of_range() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => RegisterBits.IsSet(0, 16));

    [Fact]
    public void ToBits_maps_set_bits()
    {
        var bits = RegisterBits.ToBits(0xA005);
        Assert.True(bits[0]);
        Assert.True(bits[2]);
        Assert.True(bits[13]);
        Assert.True(bits[15]);
        Assert.False(bits[1]);
    }
}
