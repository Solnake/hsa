using System.Net;
using System.Text;
using System.Web;

namespace lab3;

public class GoogleAnalyticsApi
{
    public static void TrackEvent(string category, string action, string label, int? value = null)
    {
        Track(category, action, label, value);
    }
    
    private static void Track(string category, string action, string label,
        int? value = null)
    {
        if (string.IsNullOrEmpty(category))
        {
            throw new ArgumentNullException(nameof(category));
        }

        if (string.IsNullOrEmpty(action))
        {
            throw new ArgumentNullException(nameof(action));
        }

        var request = (HttpWebRequest)WebRequest.Create("https://www.google-analytics.com/collect");
        request.Method = "POST";

        // the request body we want to send
        var postData = new Dictionary<string, string>
        {
            { "v", "1" },
            { "tid", "UA-XXXXXX-XX" },
            { "cid", "555" },
            { "t", "event" },
            { "ec", category },
            { "ea", action },
        };
        if (!string.IsNullOrEmpty(label))
        {
            postData.Add("el", label);
        }

        if (value.HasValue)
        {
            postData.Add("ev", value.ToString());
        }

        var postDataString = postData
            .Aggregate("", (data, next) => $"{data}&{next.Key}={HttpUtility.UrlEncode(next.Value)}")
            .TrimEnd('&');

        // set the Content-Length header to the correct value
        request.ContentLength = Encoding.UTF8.GetByteCount(postDataString);

        // write the request body to the request
        using (var writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.Write(postDataString);
        }

        try
        {
            var webResponse = (HttpWebResponse)request.GetResponse();
            if (webResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Google Analytics tracking did not return OK 200");
            }
        }
        catch (Exception ex)
        {
            // do what you like here, we log to Elmah
            // ElmahLog.LogError(ex, "Google Analytics tracking failed");
        }
    }
}