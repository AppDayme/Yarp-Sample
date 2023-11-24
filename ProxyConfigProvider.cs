using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Yarp.ReverseProxy.Configuration;

namespace YarpExample
{
    public static class GlobalConfig
    {
        //Flag indicating whether the config has changed
        public static bool ConfigChanged { get; set; }
    }
    public class ProxyConfigProvider : IProxyConfigProvider
    {
        
        private readonly IOptionsMonitor<ProxyConfig> _proxyConfigMonitor;
        private ProxyConfig _config;
        

        public ProxyConfigProvider(IOptionsMonitor<ProxyConfig> proxyConfigMonitor)
        {
          //  _config = _proxyConfigMonitor.CurrentValue;
           // _config.ChangeToken.RegisterChangeCallback(state => UpdateConfig(), _config);
        }
   

        public ProxyConfig ReloadConfig()
        {
            // Force a reload (invokes the callback registered with OnChange)
            _proxyConfigMonitor.Get("Reload");

            return _config;
        }




        public IProxyConfig GetConfig()
        {

            if (GlobalConfig.ConfigChanged)
            {

                UpdateConfig(); 

            }
            else
            {
                _config = new ProxyConfig(ConfigureRoute(), ConfigureCluster());
            }
             
                

            return _config;
        }





        private void UpdateConfig()
        {
            var newConfig = new ProxyConfig(ConfigureRoute(), ConfigureCluster());
            var oldConfig = _config;
            _config = newConfig;

            oldConfig.SignalChange();
        }




        public IReadOnlyList<RouteConfig> ConfigureRoute()
        {
            // Configure routes based on your logic
            return new[]
            {
                new RouteConfig
                {
                    RouteId = "route1",
                    ClusterId = "cluster1",
                    Match = new RouteMatch
                    {
                        Path = "/en-US/{**remainder}".ToLowerInvariant()
                    }
                }
            };
        }

        public static IReadOnlyList<ClusterConfig> ConfigureCluster()
        {
            // Configure clusters based on your logic
            var address = ServiceConfiguration.GetXmlValue("url");

            return new[]
            {
                new ClusterConfig
                {
                    ClusterId = "cluster1",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        { "destination1", new DestinationConfig { Address = address } }
                    }
                }
            };
        }
    }



    

}





