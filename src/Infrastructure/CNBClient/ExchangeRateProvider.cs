using ExchangeRate.Domain.Entities;
using ExchangeRate.Infrastructure.CNB.Core;
using ExchangeRate.Infrastructure.CNB.Core.Repositories;
using ExchangeRate.Infrastructure.Common;

namespace ExchangeRate.Infrastructure.CNBClient;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateRepository _exchangeRateRepository;

    public ExchangeRateProvider(IExchangeRateRepository exchangeRateRepository)
    {
        _exchangeRateRepository = exchangeRateRepository;
    }

    //ToDo could use middlewares for logging and global exception handling
    public async Task<IEnumerable<string>> GetExchangeRates()
    {
        //ToDo refactor this shit (appsettings maybe or entry in API and default in appsettings, if nowhere ==> validationException)
        try
        {
            var defaultTargetCurrency = new Currency("CZK");
            var data = await _exchangeRateRepository.GetExchangeRatesAsync();

            var result = FilterExchangeRates(data, defaultTargetCurrency);

            if (result is null)
                return new List<string>();

            return DelejPico(result);
        }
        catch (Exception e)
        {
            //ToDo add logging
            Console.WriteLine(e);
            throw;
        }
    }

    private static IEnumerable<Domain.Entities.ExchangeRate>? FilterExchangeRates(CNB.Core.Models.ExchangeRate? data, Currency defaultTargetCurrency)
    {
        var filter = data?.Table?.Rows?.Where(w => CommonConstants.Currencies.Select(s => s.Code).Contains(w.Code));
        var result = filter?.Select(f => new Domain.Entities.ExchangeRate(new Currency(f.Code), defaultTargetCurrency, f.Rate));

        return result;
    }

    private IEnumerable<string> DelejPico(IEnumerable<Domain.Entities.ExchangeRate> exchangeRates) => exchangeRates.Select(f => f.ToString());
}
