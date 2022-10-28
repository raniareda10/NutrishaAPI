using System.Threading.Tasks;
using DL.Repositories.MobileUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace NutrishaAPI.Controllers.V1.Mobile.Kommunicate
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class KommunicateController : SharedApiController
    {
        private readonly MobileUserRepository _mobileUserRepository;
        private readonly ILogger<KommunicateController> _logger;
        private readonly string _kommunicateSecretKey;

        public KommunicateController(MobileUserRepository mobileUserRepository,
            IConfiguration configuration,
            ILogger<KommunicateController> logger)
        {
            _mobileUserRepository = mobileUserRepository;
            _kommunicateSecretKey = configuration["Kommunicate:SecretKey"];
            _logger = logger;
        }

        [HttpPost("NewMessageArrived")]
        public async Task<IActionResult> NewMessageArrivedAsync([FromBody] KommunicateMessage message)
        {
            var hasAutHeader = HttpContext
                .Request
                .Headers
                .TryGetValue("Authentication", out var authToken);

            if (!hasAutHeader) return BadRequest();
            authToken = authToken.ToString().Replace("Basic ", "");

            if (authToken != _kommunicateSecretKey)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(message?.From))
            {
                _logger.LogWarning("Something is wrong with Kommunicate please contact your admin");
                return BadRequest();
            }

            var isValidUserId = int.TryParse(message.From, out var userId);

            if (!isValidUserId || userId <= 0)
            {
                _logger.LogWarning("Something is wrong with Kommunicate please contact your admin");
                return BadRequest();
            }

            await _mobileUserRepository.UserSentMessageAsync(userId, message.Message);
            return Ok();
        }

        public class KommunicateMessage
        {
            public string Key { get; set; }
            public string From { get; set; }
            public string GroupId { get; set; }
            public string ClientGroupId { get; set; }
            public string GroupName { get; set; }
            public string Message { get; set; }
            public object File { get; set; }
        }
    }
}