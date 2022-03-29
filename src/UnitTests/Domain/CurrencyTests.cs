using ExchangeRate.Domain.Entities;
using Xunit;

namespace ExchangeRate.UnitTests.Domain;

public class CurrencyTests
{
    [Fact]
    public void ShouldReturnCurrencyCode()
    {
        var expected = "test";
        var curr = new Currency(expected);

        var result = curr.ToString();

        Assert.Equal(expected, result);
    }
}
