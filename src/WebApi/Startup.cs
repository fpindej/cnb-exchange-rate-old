using System.Net;
using ExchangeRate.Infrastructure.CNB.Client;
using ExchangeRate.Infrastructure.CNB.Client.Extensions;
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
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        Log.Debug("ConfigureServices => Setting AddControllers");
        services.AddControllers();

        Log.Debug("ConfigureServices => Setting AddHealthChecks");
        services.AddHealthChecks();

        Log.Debug("ConfigureServices => Setting AddSwaggerGen");
        services.AddSwaggerGen();

        

        Log.Debug("ConfigureServices => Setting CnbClient");
        services.AddCnbClient();
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

    private T GetConfigurationValue<T>(string key, T defaultValue)
    {
        var value = _configuration[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            Log.Warning("No value for key {Key}. Using default value of {Value}", key, defaultValue);
            return defaultValue;
        }

        var res = (T)Convert.ChangeType(value, typeof(T));
        Log.Information("Getting default value for {Key} => {Value}", key, res);
        return res;
    }
}
