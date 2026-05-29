using System;
using WireCap;
using Xunit;

public class RegisterRangeTests
{
    [Fact]
    public void Valid_range_exposes_end() => Assert.Equal(109, new RegisterRange(100, 10).End);

    [Fact]
    public void Zero_count_throws() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new RegisterRange(0, 0));

    [Fact]
    public void Over_125_throws() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new RegisterRange(0, 126));

    [Fact]
    public void Overflow_past_address_space_throws() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new RegisterRange(65535, 2));
}
