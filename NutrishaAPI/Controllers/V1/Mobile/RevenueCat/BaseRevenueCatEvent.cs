using Newtonsoft.Json;

namespace NutrishaAPI.Controllers.V1.Mobile.RevenueCat
{
    public class BaseRevenueCatEvent
    {
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("app_user_id")] public int AppUserId { get; set; }
        [JsonProperty("original_app_user_id")] public int OriginalAppUserId { get; set; }
    }
}