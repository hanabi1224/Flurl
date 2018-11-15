using System.Net;
using System.Net.Http;
using Flurl.Http.Configuration;

namespace Flurl.Http
{
    /// <summary>
    /// Let Flurl.Http prefer http/2 when possible
    /// </summary>
    public class Http2ClientFactory : DefaultHttpClientFactory
    {
        /// <inheritdoc />
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new Http2MessageHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };
        }
    }

    /// <summary>
    /// Fluent API provider static class
    /// </summary>
    public static class Http2ClientFactoryExtensions
    {
        /// <summary>
        /// Fluent API to enable http/2
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IFlurlRequest EnableHttp2(this IFlurlRequest request)
        {
            request.CustomHttpClientFactory = new Http2ClientFactory();
            return request;
        }

        /// <summary>
        /// Fluent API to enable http/2
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IFlurlRequest EnableHttp2(this Url url)
        {
            return new FlurlRequest(url).EnableHttp2();
        }

        /// <summary>
        /// Fluent API to enable http/2
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IFlurlRequest EnableHttp2(this string url)
        {
            return new Url(url).EnableHttp2();
        }
    }
}
