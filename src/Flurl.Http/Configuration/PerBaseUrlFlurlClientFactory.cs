namespace Flurl.Http.Configuration
{
    /// <summary>
    /// An IFlurlClientFactory implementation that caches and reuses the same IFlurlClient instance
    /// per URL requested, which it assumes is a "base" URL, and sets the IFlurlClient.BaseUrl property
    /// to that value. Ideal for use with IoC containers - register as a singleton, inject into a service
    /// that wraps some web service, and use to set a private IFlurlClient field in the constructor.
    /// </summary>
    public class PerBaseUrlFlurlClientFactory : FlurlClientFactoryBase
    {
        /// <inheritdoc />
        protected override string GetCacheKey(Url url, IHttpClientFactory httpClientFactory)
        {
            return $"{httpClientFactory?.GetType().FullName}\x01{url}";
        }

        /// <inheritdoc />
        protected override IFlurlClient Create(Url url) => new FlurlClient(url);
    }
}
