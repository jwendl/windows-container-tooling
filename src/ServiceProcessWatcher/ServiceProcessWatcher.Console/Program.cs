using Newtonsoft.Json;
using ServiceProcessWatcher.ETW;
using ServiceProcessWatcher.EventLog;
using ServiceProcessWatcher.ServiceManagement;
using System;
using System.IO;
using System.Linq;
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
            if (config.Etw.Any())
            {
                using (var etwWatcher = new EtwWatcher(Outputer))
                {
                    foreach (var etwConfiguration in config.Etw)
                    {
                        etwWatcher.Watch(etwConfiguration.ProviderName);
                    }

                    etwWatcher.StartListening();
                }
            }

            if (config.EventLogs.Any())
            {
                var eventLogWatcher = new EventLogWatcher(Outputer);
                foreach (var eventLog in config.EventLogs)
                {
                    eventLogWatcher.Watch(eventLog.LogName);
                }
            }

            // Watch services
            IServiceWatcher serviceWatcher = new PollingServiceWatcher();
            serviceWatcher.StartServices(config.Services, Crash);

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Crash(string reason)
        {
            System.Console.WriteLine(reason);
            Environment.Exit(1);

        }

        static void Outputer(string output)
        {
            System.Console.WriteLine(output);
        }
    }
}
