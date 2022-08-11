using Newtonsoft.Json;

namespace NutrishaAPI.Controllers.V1.Mobile.RevenueCat
{
    public class TestRevenueEvent : BaseRevenueCatEvent
    {
        [JsonProperty("entitlement_id")] public string SubscriptionPlan { get; set; }
    }
}