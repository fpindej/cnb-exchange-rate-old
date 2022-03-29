using System.ComponentModel.DataAnnotations;
using ExchangeRate.Domain.Entities;
using ExchangeRate.Infrastructure.CNB.Core;
using ExchangeRate.Infrastructure.CNB.Core.Repositories;
using ExchangeRate.Infrastructure.Common;
using Logging.Exceptions;
using Microsoft.Extensions.Configuration;

namespace ExchangeRate.Infrastructure.CNB.Client;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IConfiguration _configuration;
    private readonly IExchangeRateRepository _exchangeRateRepository;

    public ExchangeRateProvider(IExchangeRateRepository exchangeRateRepository, IConfiguration configuration)
    {
        _exchangeRateRepository = exchangeRateRepository;
        _configuration = configuration;
    }

    public async Task<IEnumerable<string>> GetExchangeRates()
    {
        var defaultTargetCurrency = GetDefaultTargetCurrency();
        var data = await _exchangeRateRepository.GetExchangeRatesAsync();

        return GetExchangeRatesStringValues(FilterExchangeRates(data, defaultTargetCurrency));
    }

    private Currency GetDefaultTargetCurrency()
    {
        var targetCurrency = _configuration.GetValue<string>("CNB:DefaultCurrency");

        if (string.IsNullOrWhiteSpace(targetCurrency))
            throw new ValidationException("Target currency cannot be empty");

        return new Currency(targetCurrency);
    }

    private static IEnumerable<Domain.Entities.ExchangeRate> FilterExchangeRates(CNB.Core.Models.ExchangeRate data, Currency defaultTargetCurrency)
    {
        if (data?.Table.Rows is null)
            throw new EmptyResultSetException("Data cannot be empty");

        var filter = data.Table.Rows.Where(w => CommonConstants.Currencies.Select(s => s.Code).Contains(w.Code));
        return filter.Select(f => new Domain.Entities.ExchangeRate(new Currency(f.Code), defaultTargetCurrency, f.Rate));
    }

    private static IEnumerable<string> GetExchangeRatesStringValues(IEnumerable<Domain.Entities.ExchangeRate> exchangeRates) => exchangeRates.Select(f => f.ToString());
}
