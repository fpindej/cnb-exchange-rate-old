using ExchangeRate.Domain.Entities;
using Xunit;

namespace ExchangeRate.UnitTests.Domain;

public class ExchangeRateTests
{
    [Fact]
    public void ShouldReturnSourceToTargetCurrencyRatio()
    {
        var currencyCode = "test";
        decimal rateValue = 5;
        var expected = $"{currencyCode}/{currencyCode}={rateValue}";
        var exchangeRate = new ExchangeRate.Domain.Entities.ExchangeRate(new Currency(currencyCode), new Currency(currencyCode), rateValue);

        var result = exchangeRate.ToString();

        Assert.Equal(expected, result);
    }
}
