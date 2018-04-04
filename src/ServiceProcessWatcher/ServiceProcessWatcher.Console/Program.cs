using Newtonsoft.Json;
using ServiceProcessWatcher.ETW;
using ServiceProcessWatcher.EventLog;
using ServiceProcessWatcher.ServiceManagement;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceProcessWatcher.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Outputer("Starting watcher process..");

            // Read configuration
            var configPath = args[0];

            Outputer($"Loading config path from {configPath}");
            var configText = File.ReadAllText(configPath);
            var config = JsonConvert.DeserializeObject<Configuration>(configText);

            // TODO: Set up logs
            if (config.Etw.Any())
            {
                Outputer($"Configuring ETW providers...");
                using (var etwWatcher = new EtwWatcher(Outputer))
                {
                    foreach (var etwConfiguration in config.Etw)
                    {
                        Outputer($"Adding Provider: {etwConfiguration.ProviderName}");
                        etwWatcher.Watch(etwConfiguration.ProviderName);
                    }

                    Task.Run(() => etwWatcher.StartListening());
                }
            }

            if (config.EventLogs.Any())
            {
                Outputer($"Configuring EventLogs...");
                var eventLogWatcher = new EventLogWatcher(Outputer);
                foreach (var eventLog in config.EventLogs)
                {
                    Outputer($"Adding EventLog: {eventLog.LogName}");
                    eventLogWatcher.Watch(eventLog.LogName);
                }
            }

            // Watch services
            Outputer($"Starting services");
            IServiceWatcher serviceWatcher = new PollingServiceWatcher();
            serviceWatcher.StartServices(config.Services, Crash);

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Crash(string reason)
        {
            Outputer("Service stopped");
            Outputer(reason);
            Environment.Exit(1);

        }

        static void Outputer(string output)
        {
            System.Console.WriteLine(output);
        }
    }
}
