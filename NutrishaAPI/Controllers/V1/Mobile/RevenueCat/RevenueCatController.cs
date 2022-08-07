using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using DL.Repositories.MobileUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile.RevenueCat
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class RevenueCatController : ControllerBase
    {
        private readonly MobileUserRepository _mobileUserRepository;

        public RevenueCatController(MobileUserRepository mobileUserRepository)
        {
            _mobileUserRepository = mobileUserRepository;
        }

        [HttpPost("Event")]
        public async Task<IActionResult> PostAsync([FromBody] JObject json)
        {
            var apiVersion = json.GetValue("api_version");
            if (apiVersion.ToString() != "1.0") throw new Exception("Not Supported RevenueCat Event Type");
            var revenueCatEvent = json.GetValue("event") as JObject;
            var eventType = revenueCatEvent.GetValue("type")?.ToString();

            switch (eventType)
            {
                case RevenueCatEventTypes.InitialPurchase:
                {
                    var initialPurchaseEvent = revenueCatEvent.ToObject<InitialPurchaseEvent>();
                    await _mobileUserRepository.UserPayedAsync(initialPurchaseEvent.AppUserId,
                        initialPurchaseEvent.Price);
                    return Ok(initialPurchaseEvent);
                    break;
                }

                case RevenueCatEventTypes.Renewal:
                {
                    var initialPurchaseEvent = revenueCatEvent.ToObject<InitialPurchaseEvent>();
                    await _mobileUserRepository.UserPayedAsync(initialPurchaseEvent.AppUserId,
                        initialPurchaseEvent.Price);
                    return Ok(initialPurchaseEvent);
                    break;
                }
            }

            return Ok();
        }
    }

    public class BaseRevenueCatEventBody
    {
        [JsonProperty("api_version")] public float ApiVersion { get; set; }

        [JsonProperty("event")] public BaseRevenueCatEvent Event { get; set; }
    }

    public class BaseRevenueCatEvent
    {
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("app_user_id")] public int AppUserId { get; set; }
        [JsonProperty("entitlement_id")] public string EntitlementId { get; set; }
        [JsonProperty("price")] public float Price { get; set; }
    }


    public class InitialPurchaseEvent : BaseRevenueCatEvent
    {
    }

    public class TestRevenueEvent : BaseRevenueCatEvent
    {
        [JsonProperty("entitlement_id")] public string SubscriptionPlan { get; set; }
    }

    public class RevenueCatEventTypes
    {
        public const string Test = "TEST";
        public const string InitialPurchase = "INITIAL_PURCHASE";
        public const string NonRenewingPurchase = "NON_RENEWING_PURCHASE";
        public const string Renewal = "RENEWAL";
        public const string ProductChange = "PRODUCT_CHANGE";
        public const string Cancellation = "CANCELLATION";
        public const string UnCancellation = "UNCANCELLATION";
        public const string BillingIssue = "BILLING_ISSUE";
        public const string SubscriberAlias = "SUBSCRIBER_ALIAS";
        public const string SubscriptionPaused = "SUBSCRIPTION_PAUSED";
        public const string Transfer = "TRANSFER";
        public const string Expiration = "EXPIRATION";
    }
}