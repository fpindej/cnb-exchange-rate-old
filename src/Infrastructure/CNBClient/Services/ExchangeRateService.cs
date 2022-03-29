using System.Net;
using ExchangeRate.Infrastructure.CNB.Core.Services;
using Serilog;

namespace ExchangeRate.Infrastructure.CNBClient.Services;

public class ExchangeRateService : IExchangeRateService
{
    //ToDo inject IOptions or IConfiguration to get the endpoint
    private const string ExchangeRatesUrl = "cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
    private readonly HttpClient _httpClient;

    public ExchangeRateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage?> FetchDataAsync()
    {
        var response = await _httpClient.GetAsync(ExchangeRatesUrl);

        if (IsResponseInvalid(response))
        {
            Log.Fatal("Http request error: {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"Http request error: {response.StatusCode}");
        }

        return response;
    }

    private bool IsResponseInvalid(HttpResponseMessage resp)
        => resp.StatusCode is HttpStatusCode.InternalServerError
            or HttpStatusCode.BadRequest
            or HttpStatusCode.GatewayTimeout
            or HttpStatusCode.NotImplemented
            or HttpStatusCode.Unauthorized
            or HttpStatusCode.Forbidden
            or HttpStatusCode.NotFound
            or HttpStatusCode.BadGateway;
}
