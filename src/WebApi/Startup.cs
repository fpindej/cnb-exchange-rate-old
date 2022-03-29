using System.Net;
using ExchangeRate.Infrastructure.CNB.Client;
using ExchangeRate.Infrastructure.CNB.Client.Repositories;
using ExchangeRate.Infrastructure.CNB.Client.Services;
using ExchangeRate.Infrastructure.CNB.Core;
using ExchangeRate.Infrastructure.CNB.Core.Repositories;
using ExchangeRate.Infrastructure.CNB.Core.Services;
using Logging.Middlewares;
using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace ExchangeRate.WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        Log.Debug("ConfigureServices => Setting AddControllers");
        services.AddControllers();

        Log.Debug("ConfigureServices => Setting AddHealthChecks");
        services.AddHealthChecks();

        Log.Debug("ConfigureServices => Setting AddSwaggerGen");
        services.AddSwaggerGen();

        Log.Debug("ConfigureServices => Setting IExchangeRateRepository");
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();

        Log.Debug("ConfigureServices => Setting IExchangeRateProvider");
        services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();

        Log.Debug("ConfigureServices => Setting IExchangeRateService");

        services.AddHttpClient<IExchangeRateService, ExchangeRateService>(opt => { opt.BaseAddress = new Uri(Configuration["CNB:BaseUrl"]); })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy(GetRetryCount()));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Log.Debug("IsDevelopment: {IsDevelopment}", env.IsDevelopment());

        if (env.IsDevelopment())
        {
            Log.Debug("Using UseDeveloperExceptionPage");
            app.UseDeveloperExceptionPage();

            Log.Debug("Setting cors => allow *");

            app.UseCors(builder =>
            {
                builder.SetIsOriginAllowed(_ => true);
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
        }

        Log.Debug("Setting Exception handling middleware");
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        Log.Debug("Setting UseSwagger");
        app.UseSwagger();

        Log.Debug("Setting UseSwaggerUI");
        app.UseSwaggerUI();

        Log.Debug("Setting UseHttpsRedirection");
        app.UseHttpsRedirection();

        Log.Debug("Setting UseRouting");
        app.UseRouting();

        Log.Debug("Setting UseEndpoints");

        app.UseEndpoints(endpoints =>
        {
            Log.Debug("Setting endpoints => MapControllers");
            endpoints.MapControllers();

            Log.Debug("Setting endpoints => add health check");
            endpoints.MapHealthChecks("/health");
        });
    }

    private int GetRetryCount()
    {
        if (!int.TryParse(Configuration["CNB:RetryCount"], out var retryCount))
        {
            retryCount = 5;
            Log.Warning("No retry count specified. Using default value of 5");
        }

        return retryCount;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount) => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
