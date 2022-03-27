using ExchangeRate.Infrastructure.CNB.Core.Repositories;
using ExchangeRate.Infrastructure.CNB.Core.Services;
using ExchangeRate.Infrastructure.CNBClient.Repositories;
using ExchangeRate.Infrastructure.CNBClient.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IExchangeRateRepository, ExchangeRateRepository>();
builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>(opt => { opt.BaseAddress = new Uri(builder.Configuration["CNB:BaseUrl"]); });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
