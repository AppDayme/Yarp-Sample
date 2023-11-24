using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy.Configuration;

namespace YarpExample
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddSingleton<IProxyConfigProvider, ProxyConfigProvider>();
            builder.Services.AddReverseProxy();

            builder.Services.AddSingleton<ConfigFileWatcher>(provider =>
                new ConfigFileWatcher(
                    Path.Combine(AppContext.BaseDirectory),

             provider.GetRequiredService<IProxyConfigProvider>()
                )

            );

            var app = builder.Build();
            
            // Access the ConfigFileWatcher instance
            var configFileWatcher = app.Services.GetRequiredService<ConfigFileWatcher>();

            // Configure the event handler for the ConfigFileChanged event
            configFileWatcher.ConfigFileChanged += () =>
            {

                // Perform actions when the config file changes
              //  Console.WriteLine("Config file changed!");
            };

            app.UseRouting();
            app.MapReverseProxy();
            app.Run();
        }
    }
}