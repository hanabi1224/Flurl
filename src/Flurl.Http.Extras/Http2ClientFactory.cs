using System.Net;
using System.Net.Http;
using Flurl.Http.Configuration;

namespace Flurl.Http
{
    public class Http2ClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new Http2MessageHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };
        }
    }

    public static class Http2ClientFactoryExtensions
    {
        public static IFlurlRequest EnableHttp2(this IFlurlRequest request)
        {
            request.CustomHttpClientFactory = new Http2ClientFactory();
            return request;
        }
    }
}
