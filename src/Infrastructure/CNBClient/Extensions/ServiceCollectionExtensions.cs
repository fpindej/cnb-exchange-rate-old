using System.Net;
using ExchangeRate.Infrastructure.CNB.Client.Repositories;
using ExchangeRate.Infrastructure.CNB.Client.Services;
using ExchangeRate.Infrastructure.CNB.Core;
using ExchangeRate.Infrastructure.CNB.Core.Repositories;
using ExchangeRate.Infrastructure.CNB.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace ExchangeRate.Infrastructure.CNB.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCnbClient(this IServiceCollection services)
    {
        services.AddOptions<CnbConfiguration>()
            .BindConfiguration(CnbConfiguration.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddRepositories();

        Log.Debug("ConfigureServices => Setting IExchangeRateService");
        services.AddHttpClient<IExchangeRateService, ExchangeRateService>((sp, opt) =>
            {
                var cnbConfiguration = sp.GetRequiredService<IOptions<CnbConfiguration>>().Value;
                opt.BaseAddress = new Uri(cnbConfiguration.BaseUrl);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy(3));

        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        Log.Debug("ConfigureServices => Setting IExchangeRateRepository");
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();

        Log.Debug("ConfigureServices => Setting IExchangeRateProvider");
        services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();

        return services;
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount) => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
