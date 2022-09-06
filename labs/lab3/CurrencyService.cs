
using System.Text.Json;
using System.Text.Json.Serialization;

namespace lab3;

public class CurrencyService
{
    private readonly GoogleAnalyticsApi _analytics;

    public CurrencyService(GoogleAnalyticsApi analytics)
    {
        _analytics = analytics;
    }

    public async Task SendToGaAsync()
    {
        var rate = GetRateAsync();
        await _analytics.Track("currency", rate.ToString());
    }

    public async Task<decimal> GetRateAsync()
    {
        using var client = new HttpClient();
        var response = await client.GetStreamAsync("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");
        var rates = await JsonSerializer.DeserializeAsync<List<ChangeRate>>(response);
        return rates.Find(r => r.Currency.Equals("USD")).Rate;
    }

    private class ChangeRate
    {
        [JsonPropertyName("cc")]
        public string Currency { get; set; }
        
        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}

