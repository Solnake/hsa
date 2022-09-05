using Microsoft.AspNetCore.Mvc;

namespace lab3.Controllers;

[ApiController]
[Route("[controller]")]
public class CurrencyController
{

    private readonly CurrencyService _service;

    public CurrencyController(CurrencyService service)
    {
        _service = service;
    }

    [HttpGet(Name = "GetCurrency")]
    public async Task<decimal> GetAsync()
    {
        return await _service.GetRateAsync();
    }
}