namespace ExchangeRate.Infrastructure.CNB.Core.Repositories;

public interface IExchangeRateRepository
{
    Task<string> GetExchangeRatesAsync();
}
