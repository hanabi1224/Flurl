using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Flurl.Http.Configuration;

namespace Flurl.Http
{
    /// <summary>
    /// Provides mTLS authentication support
    /// </summary>
    public class MtlsHttpClientFactory : DefaultHttpClientFactory
    {
        private readonly X509Certificate2 clientCertificate;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientCertificate">Cleint certificate for mTLS authentication</param>
        public MtlsHttpClientFactory(X509Certificate2 clientCertificate)
        {
            this.clientCertificate = clientCertificate ?? throw new ArgumentNullException(nameof(clientCertificate));
        }

        /// <inheritdoc />
        public override HttpMessageHandler CreateMessageHandler()
        {
            var handler = new HttpClientHandler
            {
                // #266
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ClientCertificates.Add(clientCertificate);

            return handler;
        }
    }

    /// <summary>
    /// Fluent API provider static class
    /// </summary>
    public static class MtlsHttpClientFactoryExtensions
    {
        /// <summary>
        /// Fluent API to specify client certificate for mTLS authentication
        /// </summary>
        /// <param name="request"></param>
        /// <param name="clientCertificate"></param>
        /// <returns></returns>
        public static IFlurlRequest WithMtlsClientCertificate(
            this IFlurlRequest request,
            X509Certificate2 clientCertificate)
        {
            request.CustomHttpClientFactory = new MtlsHttpClientFactory(clientCertificate);
            return request;
        }

        /// <summary>
        /// Fluent API to specify client certificate for mTLS authentication
        /// </summary>
        /// <param name="request"></param>
        /// <param name="clientCertificate"></param>
        /// <returns></returns>
        public static IFlurlRequest WithMtlsClientCertificate(
            this Url url,
            X509Certificate2 clientCertificate)
        {
            return new FlurlRequest(url).WithMtlsClientCertificate(clientCertificate);
        }

        /// <summary>
        /// Fluent API to specify client certificate for mTLS authentication
        /// </summary>
        /// <param name="request"></param>
        /// <param name="clientCertificate"></param>
        /// <returns></returns>
        public static IFlurlRequest WithMtlsClientCertificate(
            this string url,
            X509Certificate2 clientCertificate)
        {
            return new FlurlRequest(url).WithMtlsClientCertificate(clientCertificate);
        }
    }
}
