using System;
using System.Collections.Concurrent;

namespace Flurl.Http.Configuration
{
    /// <summary>
    /// Encapsulates a creation/caching strategy for IFlurlClient instances. Custom factories looking to extend
    /// Flurl's behavior should inherit from this class, rather than implementing IFlurlClientFactory directly.
    /// </summary>
    public abstract class FlurlClientFactoryBase : IFlurlClientFactory
    {
        private readonly ConcurrentDictionary<string, IFlurlClient> _clients = new ConcurrentDictionary<string, IFlurlClient>();

        /// <inheritdoc />
        public virtual IFlurlClient Get(Url url, IHttpClientFactory httpClientFactory = null)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

            return _clients.AddOrUpdate(
                GetCacheKey(url, httpClientFactory),
                u => Create(u, httpClientFactory),
                (u, client) => client.IsDisposed ? Create(u, httpClientFactory) : client);
        }

        /// <summary>
        /// Defines a strategy for getting a cache key based on a Url. Default implementation
        /// returns the host part (i.e www.api.com) so that all calls to the same host use the
        /// same FlurlClient (and HttpClient/HttpMessageHandler) instance.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="httpClientFactory">Custom http client factory, null as default</param>
        /// <returns>The cache key</returns>
        protected abstract string GetCacheKey(Url url, IHttpClientFactory httpClientFactory);

        /// <summary>
        /// Creates a new FlurlClient
        /// </summary>
        /// <param name="url">The URL (not used)</param>
        /// <param name="httpClientFactory">Custom http client factory, null as default</param>
        /// <returns></returns>
        protected virtual IFlurlClient Create(Url url, IHttpClientFactory httpClientFactory)
        {
            if (httpClientFactory == null)
            {
                return new FlurlClient();
            }
            else
            {
                var httpClient = httpClientFactory.CreateHttpClient(httpClientFactory.CreateMessageHandler());
                return new FlurlClient(httpClient);
            }
        }

        /// <summary>
        /// Disposes all cached IFlurlClient instances and clears the cache.
        /// </summary>
        public void Dispose()
        {
            foreach (var kv in _clients)
            {
                if (!kv.Value.IsDisposed)
                    kv.Value.Dispose();
            }
            _clients.Clear();
        }
    }
}
