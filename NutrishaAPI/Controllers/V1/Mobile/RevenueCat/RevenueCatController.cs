using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DL.EntitiesV1.Payments;
using DL.Repositories.MobileUser;
using DL.Repositories.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NutrishaAPI.Controllers.V1.Mobile.RevenueCat
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class RevenueCatController : ControllerBase
    {
        private readonly MobileUserRepository _mobileUserRepository;
        private readonly PaymentHistoryService _paymentHistoryService;
        private readonly ILogger<RevenueCatController> _logger;
        private readonly string _revenueCatSecretKey;

        public RevenueCatController(MobileUserRepository mobileUserRepository,
            IConfiguration configuration,
            PaymentHistoryService paymentHistoryService,
            ILogger<RevenueCatController> logger)
        {
            _mobileUserRepository = mobileUserRepository;
            _paymentHistoryService = paymentHistoryService;
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
                return Unauthorized();
            }

            if (body.ApiVersion != "1.0" || body.Event == null)
            {
                _logger.LogError("RevenueCat Start Using ApiVersion That Not Supported.");
                return Ok();
            }

            var eventType = body.Event?.GetValue("type")?.ToString();
            var revenueCatEvent = body.Event.ToObject<BaseRevenueCatEvent>();
            var paymentHistory = new PaymentHistoryEntity()
            {
                PaymentId = revenueCatEvent.Id,
                UserId = revenueCatEvent.AppUserId,
                Created = DateTime.UtcNow,
                Type = eventType,
                Event = JsonConvert.SerializeObject(body.Event)
            };
            switch (eventType)
            {
                case RevenueCatEventTypes.InitialPurchase:
                case RevenueCatEventTypes.NonRenewingPurchase:
                {
                    var initialPurchaseEvent = body.Event.ToObject<InitialPurchaseEvent>();
                    await _mobileUserRepository.UserSubscribedAsync(initialPurchaseEvent.AppUserId,
                        initialPurchaseEvent.PriceInPurchasedCurrency);
                    paymentHistory.Currency = initialPurchaseEvent.Currency;
                    paymentHistory.Price = initialPurchaseEvent.Price;
                    break;
                }

                case RevenueCatEventTypes.Renewal:
                {
                    var initialPurchaseEvent = body.Event.ToObject<InitialPurchaseEvent>();
                    await _mobileUserRepository.UserPayedAsync(initialPurchaseEvent.AppUserId,
                        initialPurchaseEvent.PriceInPurchasedCurrency);
                    paymentHistory.Currency = initialPurchaseEvent.Currency;
                    paymentHistory.Price = initialPurchaseEvent.Price;
                    break;
                }

                case RevenueCatEventTypes.Cancellation:
                case RevenueCatEventTypes.ProductChange:
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

            await _paymentHistoryService.AddAsync(paymentHistory);
            return Ok();
        }
    }
}