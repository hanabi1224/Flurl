using System;
using System.Runtime.CompilerServices;

namespace Flurl.Http.Configuration
{
    /// <summary>
    /// Interface for defining a strategy for creating, caching, and reusing IFlurlClient instances and,
    /// by proxy, their underlying HttpClient instances.
    /// </summary>
    public interface IFlurlClientFactory : IDisposable
    {
        /// <summary>
        /// Strategy to create a FlurlClient or reuse an exisitng one, based on URL being called.
        /// </summary>
        /// <param name="url">The URL being called.</param>
        /// <param name="httpClientFactory">Custom IHttpClientFactory, null as default. TODO: Disalow null value to determine factory early to avoid race condition since HttpClient is created lazily.</param>
        /// <returns></returns>
        IFlurlClient Get(Url url, IHttpClientFactory httpClientFactory = null);
    }

    /// <summary>
    /// Extension methods on IFlurlClientFactory
    /// </summary>
    public static class FlurlClientFactoryExtensions
    {
        // https://stackoverflow.com/questions/51563732/how-do-i-lock-when-the-ideal-scope-of-the-lock-object-is-known-only-at-runtime
        private static readonly ConditionalWeakTable<IFlurlClient, object> _clientLocks = new ConditionalWeakTable<IFlurlClient, object>();

        /// <summary>
        /// Provides thread-safe access to a specific IFlurlClient, typically to configure settings and default headers.
        /// The URL is used to find the client, but keep in mind that the same client will be used in all calls to the same host by default.
        /// </summary>
        /// <param name="factory">This IFlurlClientFactory.</param>
        /// <param name="url">the URL used to find the IFlurlClient.</param>
        /// <param name="configAction">the action to perform against the IFlurlClient.</param>
        public static IFlurlClientFactory ConfigureClient(this IFlurlClientFactory factory, string url, Action<IFlurlClient> configAction)
        {
            var client = factory.Get(url, null);
            lock (_clientLocks.GetOrCreateValue(client))
            {
                configAction(client);
            }
            return factory;
        }
    }
}
