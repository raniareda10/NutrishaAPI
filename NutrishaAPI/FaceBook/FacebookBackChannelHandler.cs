using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace NutrishaAPI.FaceBook
{

        public class FacebookBackChannelHandler : HttpClientHandler
        {
            protected override async Task<HttpResponseMessage>
                SendAsync(HttpRequestMessage request,
                          CancellationToken cancellationToken)
            {
                if (!request.RequestUri.AbsolutePath.Contains("/oauth"))
                {
                    request.RequestUri = new Uri(
                        request.RequestUri.AbsoluteUri.Replace("?access_token", "&access_token"));
                }
                return await base.SendAsync(request, cancellationToken);
            }
        }
    
}
