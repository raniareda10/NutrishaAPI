using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DL.Repositories.MobileUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace NutrishaAPI.Controllers.V1.Mobile.RevenueCat
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class RevenueCatController : ControllerBase
    {
        private readonly MobileUserRepository _mobileUserRepository;
        private readonly ILogger<RevenueCatController> _logger;
        private readonly string _revenueCatSecretKey;

        public RevenueCatController(MobileUserRepository mobileUserRepository,
            IConfiguration configuration,
            ILogger<RevenueCatController> logger)
        {
            _mobileUserRepository = mobileUserRepository;
            _revenueCatSecretKey = configuration["RevenueCat:SecretKey"];
            _logger = logger;
        }

        [HttpPost("Event")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> PostAsync([FromBody] RevenueCatEventBody body)
        {
            var hasAutHeader = HttpContext
                .Request
                .Headers
                .TryGetValue(HeaderNames.Authorization, out var authToken);

            authToken = authToken.ToString().Replace("Bearer ", "");
            if (!hasAutHeader || authToken != _revenueCatSecretKey)
            {
                HttpContext.Response.Headers.Add("Auth", authToken);
                HttpContext.Response.Headers.Add("revunce_cat", _revenueCatSecretKey);
                return BadRequest();
            }

            if (body.ApiVersion != "1.0" || body.Event == null)
            {
                _logger.LogError("RevenueCat Start Using ApiVersion Than Not Supported.");
                return Ok();
            }

            var eventType = body.Event?.GetValue("type")?.ToString();
            switch (eventType)
            {
                case RevenueCatEventTypes.InitialPurchase:
                {
                    var initialPurchaseEvent = body.Event.ToObject<InitialPurchaseEvent>();
                    await _mobileUserRepository.UserSubscribedAsync(initialPurchaseEvent.AppUserId,
                        initialPurchaseEvent.PriceInPurchasedCurrency);
                    break;
                }

                case RevenueCatEventTypes.Renewal:
                {
                    var initialPurchaseEvent = body.Event.ToObject<InitialPurchaseEvent>();
                    await _mobileUserRepository.UserPayedAsync(initialPurchaseEvent.AppUserId,
                        initialPurchaseEvent.PriceInPurchasedCurrency);
                    break;
                }

                case RevenueCatEventTypes.Cancellation:
                {
                    var initialPurchaseEvent = body.Event.ToObject<BaseRevenueCatEvent>();
                    await _mobileUserRepository.UserUnSubscribedAsync(initialPurchaseEvent.AppUserId);
                    break;
                }

                case RevenueCatEventTypes.SubscriptionPaused:
                {
                    var initialPurchaseEvent = body.Event.ToObject<BaseRevenueCatEvent>();
                    await _mobileUserRepository.UserUnSubscribedAsync(initialPurchaseEvent.AppUserId);
                    break;
                }

                case RevenueCatEventTypes.Expiration:
                {
                    var initialPurchaseEvent = body.Event.ToObject<BaseRevenueCatEvent>();
                    await _mobileUserRepository.UserUnSubscribedAsync(initialPurchaseEvent.AppUserId);
                    break;
                }

                case null:
                {
                    _logger.LogError("Critical: RevenueCat Request Body Changed.");
                    break;
                }
            }

            return Ok();
        }
    }
}