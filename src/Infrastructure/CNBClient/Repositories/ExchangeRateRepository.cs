using ExchangeRate.Infrastructure.CNB.Core.Repositories;
using ExchangeRate.Infrastructure.CNB.Core.Services;
using ExchangeRate.Infrastructure.Common.Helper;

namespace ExchangeRate.Infrastructure.CNBClient.Repositories;

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateRepository(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    public async Task<CNB.Core.Models.ExchangeRate?> GetExchangeRatesAsync()
    {
        var response = await _exchangeRateService.FetchDataAsync();
        
        //ToDo if response is 200 but empty log and throw exception, otherwise just go on
        
        var content = await response.Content.ReadAsStringAsync();

        
        // ToDo put this to separete private method
        try
        {
            return content.FromXml<CNB.Core.Models.ExchangeRate>();
        }
        catch (Exception e)
        {
            //log and throw
            Console.WriteLine(e);
            throw;
        }
    }
}
