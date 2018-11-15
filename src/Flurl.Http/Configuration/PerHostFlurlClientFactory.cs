namespace Flurl.Http.Configuration
{
    /// <summary>
    /// An IFlurlClientFactory implementation that caches and reuses the same one instance of
    /// FlurlClient per host being called. Maximizes reuse of underlying HttpClient/Handler
    /// while allowing things like cookies to be host-specific. This is the default
    /// implementation used when calls are made fluently off Urls/strings.
    /// </summary>
    public class PerHostFlurlClientFactory : FlurlClientFactoryBase
    {
        /// <inheritdoc />
        protected override string GetCacheKey(Url url, IHttpClientFactory httpClientFactory)
        {
            return $"{httpClientFactory?.GetType().FullName}\x01{url.ToUri().Host}";
        }
    }
}
