namespace ExchangeRate.Infrastructure.CNB.Core.Services;

public interface IExchangeRateService
{
    Task<HttpResponseMessage?> FetchDataAsync();
}
