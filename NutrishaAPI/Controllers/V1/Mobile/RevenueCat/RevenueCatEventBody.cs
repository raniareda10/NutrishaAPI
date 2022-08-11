using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NutrishaAPI.Controllers.V1.Mobile.RevenueCat
{
    public class RevenueCatEventBody
    {
        [JsonProperty("api_version")] public string ApiVersion { get; set; }

        // I am using JObject as event object is dynamic based on the event Revenue Sent 
        [JsonProperty("event")] public JObject Event { get; set; }
    }
}