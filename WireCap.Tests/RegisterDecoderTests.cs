using WireCap;
using Xunit;

public class RegisterDecoderTests
{
    [Fact]
    public void ToUInt32_HighWordFirst_combines() =>
        Assert.Equal(0x12345678u, RegisterDecoder.ToUInt32(new ushort[] { 0x1234, 0x5678 }));

    [Fact]
    public void ToUInt32_LowWordFirst_swaps() =>
        Assert.Equal(0x12345678u, RegisterDecoder.ToUInt32(new ushort[] { 0x5678, 0x1234 }, 0, WordOrder.LowWordFirst));

    [Fact]
    public void ToInt32_handles_negative() =>
        Assert.Equal(-1, RegisterDecoder.ToInt32(new ushort[] { 0xFFFF, 0xFFFF }));

    [Fact]
    public void ToFloat_decodes_ieee754() =>
        Assert.Equal(123.456f, RegisterDecoder.ToFloat(new ushort[] { 0x42F6, 0xE979 }), 3);

    [Fact]
    public void ToUInt32_throws_when_too_short() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => RegisterDecoder.ToUInt32(new ushort[] { 0x0001 }));
}
