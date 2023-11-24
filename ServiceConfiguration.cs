using System;
using System.IO;
using System.Xml;

namespace YarpExample
{
    public static class ServiceConfiguration
    {
        private static readonly XmlDocument doc = new();
        private static readonly object lockObject = new();
     //   private static readonly FileWatcher fileWatcher;
        private static string configFile;

        public static void LoadXmlConfiguration()
        {
            configFile = Path.Combine(AppContext.BaseDirectory, "config.xml");

            // Load initial configuration
            //   ReloadConfiguration();

           // var watcher = new FileWatcher(Path.Combine(AppContext.BaseDirectory, "config.xml"), () => provider.GetRequiredService<IConfigProvider>().ReloadConfig());

            var yarpConfiguration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddXmlFile("config.xml", optional: false, reloadOnChange: true)
                            .Build();

           
            doc.LoadXml(yarpConfiguration.GetSection("AppConfiguration").Value!);
  
        }

        public static void ReloadConfiguration()
        {
            lock (lockObject)
            {
                var yarpConfiguration = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddXmlFile("config.xml")
                          .Build();

                doc.LoadXml(yarpConfiguration.GetSection("AppConfiguration").Value!);

            }
        }

        public static string GetXmlValue(string name)
        {

            var yarpConfiguration = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddXmlFile("config.xml")
                          .Build();
            yarpConfiguration.Reload();
            var configurationSection = yarpConfiguration.GetSection("destination");

            // Get the value of the 'url' element within the 'destination' section
            var url = configurationSection[name];
            //  var url = yarpConfiguration.GetSection(name).Value;
            return url;

        }

        private static bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return true;
            }
        }






    }
}
