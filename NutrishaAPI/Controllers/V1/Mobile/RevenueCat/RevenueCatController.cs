using System;
using System.Threading.Tasks;
using DL.Repositories.MobileUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public RevenueCatController(MobileUserRepository mobileUserRepository,
            ILogger<RevenueCatController> logger)
        {
            _mobileUserRepository = mobileUserRepository;
            _logger = logger;
        }

        [HttpPost("Event")]
        public async Task<IActionResult> PostAsync([FromBody] RevenueCatEventBody body)
        {
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
                    await _mobileUserRepository.UserPayedAsync(initialPurchaseEvent.AppUserId, initialPurchaseEvent.PriceInPurchasedCurrency);
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