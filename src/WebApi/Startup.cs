using ExchangeRate.Infrastructure.CNB.Core;
using ExchangeRate.Infrastructure.CNB.Core.Repositories;
using ExchangeRate.Infrastructure.CNB.Core.Services;
using ExchangeRate.Infrastructure.CNBClient;
using ExchangeRate.Infrastructure.CNBClient.Repositories;
using ExchangeRate.Infrastructure.CNBClient.Services;
using Logging.Middlewares;
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
        services.AddHttpClient<IExchangeRateService, ExchangeRateService>(opt => { opt.BaseAddress = new Uri(Configuration["CNB:BaseUrl"]); });
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
}
