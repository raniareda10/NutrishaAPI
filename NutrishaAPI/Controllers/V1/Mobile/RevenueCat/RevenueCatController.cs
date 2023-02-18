using System;
using System.Collections.Generic;
using System.Linq;
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
                return BadRequest("RevenueCat Start Using ApiVersion That Not Supported.");
            }

            _logger.LogInformation("RevenueCat Event Handling Started. Api Version: {0} Event Body {1}",
                body.ApiVersion, body.Event.ToString());
            
            var eventType = body.Event?.GetValue("type")?.ToString();
            if (string.IsNullOrWhiteSpace(eventType))
            {
                _logger.LogError("Critical: RevenueCat Request Body Changed.");
                throw new Exception("Critical: RevenueCat Request Body Changed: cant find RevenueCat type");
            }
            
            var revenueCatEvent = body.Event.ToObject<BaseRevenueCatEvent>();
            var paymentHistory = new PaymentHistoryEntity()
            {
                PaymentId = revenueCatEvent.Id,
                UserId = revenueCatEvent.AppUserId,
                Created = DateTime.UtcNow,
                Type = eventType,
                Event = JsonConvert.SerializeObject(body.Event)
            };
            
            try
            {
                switch (eventType)
                {
                    case RevenueCatEventTypes.InitialPurchase:
                    case RevenueCatEventTypes.NonRenewingPurchase:
                    {
                        var initialPurchaseEvent = body.Event.ToObject<InitialPurchaseEvent>();
                        await _mobileUserRepository.UserSubscribedAsync(initialPurchaseEvent.AppUserId.Value,
                            initialPurchaseEvent.PriceInPurchasedCurrency);
                        paymentHistory.Currency = initialPurchaseEvent.Currency;
                        paymentHistory.Price = initialPurchaseEvent.Price;
                        paymentHistory.IsHandled = true;
                        break;
                    }

                    case RevenueCatEventTypes.Renewal:
                    {
                        var initialPurchaseEvent = body.Event.ToObject<InitialPurchaseEvent>();
                        await _mobileUserRepository.UserRenewedAsync(initialPurchaseEvent.AppUserId.Value,
                            initialPurchaseEvent.PriceInPurchasedCurrency);
                        paymentHistory.Currency = initialPurchaseEvent.Currency;
                        paymentHistory.Price = initialPurchaseEvent.Price;
                        paymentHistory.IsHandled = true;
                        break;
                    }

                    case RevenueCatEventTypes.Cancellation:
                    case RevenueCatEventTypes.ProductChange:
                    case RevenueCatEventTypes.SubscriptionPaused:
                    case RevenueCatEventTypes.Expiration:
                    {
                        var initialPurchaseEvent = body.Event.ToObject<BaseRevenueCatEvent>();
                        await _mobileUserRepository.UserUnSubscribedAsync(initialPurchaseEvent.AppUserId.Value);
                        paymentHistory.IsHandled = true;
                        break;
                    }

                    case RevenueCatEventTypes.Transfer:
                    {
                        var transferEvent = body.Event.ToObject<RevenueCatTransferEvent>();
                        var transferTo = GetUserId(transferEvent.TransferredTo);
                        var transferFrom = GetUserId(transferEvent.TransferredFrom);
                        paymentHistory.UserId = transferTo;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                await _paymentHistoryService.AddAsync(paymentHistory);
                _logger.LogError("Critical: Unhandled RevenueCat Event. Api Version: {0} Event Body {1}, Exception: {2}",
                    body.ApiVersion, body.Event.ToString(), JsonConvert.SerializeObject(e));
                throw;
            }
            
            await _paymentHistoryService.AddAsync(paymentHistory);
            return Ok();
        }

        private int GetUserId(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                if (int.TryParse(id, out var appId))
                {
                    return appId;
                }
            }

            return 0;
        }
    }
}