using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ServiceProcessWatcher.ETW;
using ServiceProcessWatcher.ETW.Interfaces;
using ServiceProcessWatcher.Logging;
using ServiceProcessWatcher.Logging.Interfaces;
using ServiceProcessWatcher.ServiceManagement;
using ServiceProcessWatcher.ServiceManagement.Interfaces;
using System.IO;
using System.Threading;

namespace ServiceProcessWatcher.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read configuration
            var configPath = args[0];
            var configText = File.ReadAllText(configPath);
            var config = JsonConvert.DeserializeObject<Configuration>(configText);

            // TODO: Set up logs
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ILoggingProvider, LoggingProvider>((sp) =>
            {
                return new LoggingProvider(System.Console.Out);
            });
            serviceCollection.AddScoped<IServiceWatcher, PollingServiceWatcher>();
            serviceCollection.AddScoped<IEtwWatcher, EtwWatcher>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // ETW
            using (var etwWatcher = serviceProvider.GetRequiredService<IEtwWatcher>())
            {
                etwWatcher.Watch("Microsoft-Demos-MySource");
                //etwWatcher.Watch("Microsoft-Windows-IIS-Logging");
                etwWatcher.StartListening();
            }

            // Watch services
            var serviceWatcher = serviceProvider.GetRequiredService<IServiceWatcher>();
            serviceWatcher.StartServices(config.Services);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
