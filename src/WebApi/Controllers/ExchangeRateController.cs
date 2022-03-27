using ExchangeRate.Infrastructure.CNB.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRate.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeRateController : ControllerBase
{
    private readonly IExchangeRateRepository _exchangeRateRepository;

    public ExchangeRateController(IExchangeRateRepository exchangeRateRepository)
    {
        _exchangeRateRepository = exchangeRateRepository;
    }

    [HttpGet]
    public async Task<ActionResult<string>> Test()
    {
        var result = await _exchangeRateRepository.GetExchangeRatesAsync();

        return Ok("Muze byt");
    }
}
