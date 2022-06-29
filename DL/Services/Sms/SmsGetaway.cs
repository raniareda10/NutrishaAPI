using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DL.Services.Sms.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DL.Services.Sms
{
    public class SmsGetaway : ISmsGetaway
    {
        private readonly SmsConfiguration _smsConfiguration;
        private readonly IHttpClientFactory _httpClientFactory;

        public SmsGetaway(SmsConfiguration smsConfiguration,
            IHttpClientFactory httpClientFactory)
        {
            _smsConfiguration = smsConfiguration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendMessageAsync(string message, string to)
        {
            var body = new
            {
                body = message,
                to = to,
                from = _smsConfiguration.From
            };

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _smsConfiguration.AuthToken);
            request.Content =
                new StringContent(JsonConvert.SerializeObject(body), Encoding.Unicode, "application/json");

            request.RequestUri = new Uri($"{_smsConfiguration.Host}/messages");
            await client.SendAsync(request);
        }
    }
}