using System.Xml.Serialization;
using ExchangeRate.Infrastructure.CNB.Core.Repositories;
using ExchangeRate.Infrastructure.CNB.Core.Services;

namespace ExchangeRate.Infrastructure.CNBClient.Repositories;

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateRepository(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    public async Task<string> GetExchangeRatesAsync()
    {
        var data = await _exchangeRateService.FetchDataAsync();
        
        var xmlSerializer = new XmlSerializer(typeof(CNB.Core.Models.ExchangeRate));
        
        var rates = (CNB.Core.Models.ExchangeRate)xmlSerializer.Deserialize(data.Content.ReadAsStreamAsync().Result);

        return "lol";
    }
}
