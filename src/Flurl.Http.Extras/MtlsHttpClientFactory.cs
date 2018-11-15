using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Flurl.Http.Configuration;

namespace Flurl.Http
{
    public class MtlsHttpClientFactory : DefaultHttpClientFactory
    {
        private readonly X509Certificate2 clientCertificate;
        public MtlsHttpClientFactory(X509Certificate2 clientCertificate)
        {
            this.clientCertificate = clientCertificate ?? throw new ArgumentNullException(nameof(clientCertificate));
        }

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

    public static class MtlsHttpClientFactoryExtensions
    {
        public static IFlurlRequest WithMtlsClientCertificate(
            this IFlurlRequest request,
            X509Certificate2 clientCertificate)
        {
            request.CustomHttpClientFactory = new MtlsHttpClientFactory(clientCertificate);
            return request;
        }
    }
}
