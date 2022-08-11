using Newtonsoft.Json;

namespace NutrishaAPI.Controllers.V1.Mobile.RevenueCat
{
    public class BaseRevenueCatSubscriptionEvent : BaseRevenueCatEvent
    {
        [JsonProperty("entitlement_id")] public string EntitlementId { get; set; }
        [JsonProperty("entitlement_ids")] public string[] EntitlementIds { get; set; }
        [JsonProperty("price")] public double Price { get; set; }
        [JsonProperty("price_in_purchased_currency")] public double PriceInPurchasedCurrency { get; set; }
        [JsonProperty("currency")] public string Currency { get; set; }
    }
}