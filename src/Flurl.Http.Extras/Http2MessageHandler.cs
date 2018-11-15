using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Flurl.Http
{
    public class Http2MessageHandler : WinHttpHandler
    {
        private static readonly Version version20 = Version.Parse("2.0");
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Version = version20;
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (response.Version.Major < 2)
            {
                Trace.TraceWarning($"http/2 is not supported on current OS. Fallback to version {response.Version.ToString()}");
            }

            return response;
        }
    }
}
