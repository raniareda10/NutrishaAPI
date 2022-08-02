using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile.RevenueCat
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class RevenueCatController : ControllerBase
    {
        [HttpPost("Event")]
        [AllowBuffering]
        public async Task<IActionResult> PostAsync([FromBody] JsonElement json)
        {
            HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            using var stringReader = new StreamReader(HttpContext.Request.Body);
            var value = await stringReader.ReadToEndAsync();
            return Ok(value);
            var body = json.GetRawText();
            var baseEvent = JsonConvert.DeserializeObject<BaseRevenueCatEventBody>(body);
            return Ok(baseEvent);
        }
    }

    public class AllowBufferingAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Request.EnableBuffering();
            return base.OnActionExecutionAsync(context, next);
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
        [JsonProperty("app_user_id")] public string AppUserId { get; set; }
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