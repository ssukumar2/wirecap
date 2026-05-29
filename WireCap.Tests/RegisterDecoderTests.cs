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

public class RegisterDecoder64Tests
{
    [Fact]
    public void ToUInt64_HighWordFirst_combines() =>
        Assert.Equal(0x0011223344556677ul,
            RegisterDecoder.ToUInt64(new ushort[] { 0x0011, 0x2233, 0x4455, 0x6677 }));

    [Fact]
    public void ToInt64_handles_negative() =>
        Assert.Equal(-1L, RegisterDecoder.ToInt64(new ushort[] { 0xFFFF, 0xFFFF, 0xFFFF, 0xFFFF }));

    [Fact]
    public void ToDouble_decodes_ieee754() =>
        Assert.Equal(1.0, RegisterDecoder.ToDouble(new ushort[] { 0x3FF0, 0x0000, 0x0000, 0x0000 }), 9);

    [Fact]
    public void ToUInt64_throws_when_too_short() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => RegisterDecoder.ToUInt64(new ushort[] { 0x0001, 0x0002 }));
}
