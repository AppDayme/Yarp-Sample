using System;
using System.IO;
using Microsoft.Extensions.FileSystemGlobbing;
using Yarp.ReverseProxy.Configuration;

namespace YarpExample
{
    public class ConfigFileWatcher
    {
    //    private readonly string filePath;
    //    private  FileSystemWatcher watcher;

    //    private readonly MyServiceReplacer myServiceReplacer;


    //    // Event to be triggered when the config file changes
    //    public event Action ConfigFileChanged;

    //    public ConfigFileWatcher(string filePath)
    //    {
    //        this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    //        myServiceReplacer = new MyServiceReplacer();  
    //        // Initialize the FileSystemWatcher
    //        watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath));
    //        watcher.Filter = Path.GetFileName(filePath);
    //        watcher.NotifyFilter = NotifyFilters.LastWrite;
    //        watcher.Changed += OnFileChanged;

    //        // Begin watching
    //        watcher.EnableRaisingEvents = true;
    //        InitializeWatcher();
    //    }


         private readonly string filePath;
    private readonly IProxyConfigProvider proxyConfigProvider;

    private FileSystemWatcher watcher;

    // Event to be triggered when the config file changes
    public event Action ConfigFileChanged;

    public ConfigFileWatcher(string filePath, IProxyConfigProvider proxyConfigProvider)
    {
        this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        this.proxyConfigProvider = proxyConfigProvider ?? throw new ArgumentNullException(nameof(proxyConfigProvider));
        

        InitializeWatcher();
    }

    private void InitializeWatcher()
        {
            // Create a new FileSystemWatcher and set its properties
            watcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(filePath), // Watch the directory of the file
                Filter = Path.GetFileName(filePath),    // Watch the specific file
                NotifyFilter = NotifyFilters.LastWrite,

                // Trigger on file write/change
            };
            GlobalConfig.ConfigChanged = false; 
            // Register the event handlers
            watcher.Changed += OnFileChanged;

            // Enable the watcher
            watcher.EnableRaisingEvents = true;
        }

        // Event handler for file changes
        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
          
            
            GlobalConfig.ConfigChanged = true;
            proxyConfigProvider.GetConfig();
            GlobalConfig.ConfigChanged = false; 
                ConfigFileChanged?.Invoke();
            
        }
        

        // Clean up resources
        public void Dispose()
        {
            watcher.Dispose();
        }
    }
}
