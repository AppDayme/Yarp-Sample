using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading;
using Yarp.ReverseProxy.Configuration;

namespace YarpExample
{
    public class ProxyConfig : IProxyConfig
    {
        private CancellationTokenSource _cts;

        public ProxyConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            Routes = routes;
            Clusters = clusters;
            ResetChangeToken();
        }

        public ProxyConfig()
        {
            // Initialize any properties if needed
            ResetChangeToken();
        }

        public IReadOnlyList<RouteConfig> Routes { get; }

        public IReadOnlyList<ClusterConfig> Clusters { get; }

        public IChangeToken ChangeToken => new CancellationChangeToken(_cts.Token);

        public void SignalChange()
        {
            // Dispose the old CancellationTokenSource and create a new one
           // _cts.Cancel();
            
            ResetChangeToken();
        }

        private void ResetChangeToken()
        {
            _cts = new CancellationTokenSource();
        }

        public void Dispose()
        {

            _cts.Dispose();
        }


    }
}
