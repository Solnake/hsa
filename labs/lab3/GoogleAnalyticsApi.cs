using System.Text;
using Newtonsoft.Json;

namespace lab3;

public class GoogleAnalyticsApi
{

    private readonly string _fireBaseAppId;

    private readonly string _apiSecret;

    private readonly string _instanceId;

    public GoogleAnalyticsApi(string fireBaseAppId, string apiSecret, string instanceId)
    {
        _fireBaseAppId = fireBaseAppId;
        _apiSecret = apiSecret;
        _instanceId = instanceId;
    }

    public async Task Track(string action, string value)
    {
        if (string.IsNullOrEmpty(action))
        {
            throw new ArgumentNullException(nameof(action));
        }
        
        var client = new HttpClient();
        var url =
            $"https://www.google-analytics.com/mp/collect?firebase_app_id=${_fireBaseAppId}&api_secret=${_apiSecret}";
        var metric = new MetricBody
        {
            app_instance_id = _instanceId,
            events = new[]
            {
                new MetricEvent
                {
                    name = action,
                    @params = new[] { new KeyValuePair<string, string>("rate", value) }
                }
            }
        };

        using var bodyContent = new StringContent(JsonConvert.SerializeObject(metric), Encoding.UTF8, "application/json");
        try
        {
            var response = await client.PostAsync(url, bodyContent);
        }
        catch (Exception ex)
        {
            // do what you like here, we log to Elmah
            // ElmahLog.LogError(ex, "Google Analytics tracking failed");
        }
    }

    class MetricBody
    {
        public string app_instance_id { get; init; }
        public MetricEvent[] events { get; init; }
    }

    class MetricEvent
    {
        public string name { get; init; }
        
        public KeyValuePair<string, string>[] @params { get; init; } 
    }
}