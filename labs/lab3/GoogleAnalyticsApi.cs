using System.Text;

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
        client.BaseAddress =
            new Uri(
                $"https://www.google-analytics.com/mp/");
        
        var json = "{"+
          $@"app_instance_id: '{_instanceId}',
          events: [
            name: '{action}',"+
          "  params: {" +
          $@"value: {value}" +
          @"}
          ]
        }";
        
        var bodyContent = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await client.PostAsync($"collect?firebase_app_id=${_fireBaseAppId}&api_secret=${_apiSecret}", bodyContent);
        }
        catch (Exception ex)
        {
            // do what you like here, we log to Elmah
            // ElmahLog.LogError(ex, "Google Analytics tracking failed");
        }
    }
}