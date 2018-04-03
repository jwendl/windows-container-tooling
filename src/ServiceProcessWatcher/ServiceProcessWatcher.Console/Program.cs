using ServiceProcessWatcher.ETW;
using System;
using System.IO;
using System.Linq;
using System.Threading;

using Newtonsoft.Json;
using ServiceProcessWatcher.ServiceManagement;
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
                using (var etwWatcher = new EtwWatcher(Ouputer))
                {
                    foreach (var etwConfiguration in config.Etw)
                    {
                        etwWatcher.Watch(etwConfiguration.ProviderName);
                    }

                    etwWatcher.StartListening();
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

        static void Ouputer(string output)
        {
            System.Console.WriteLine(output);
        }
    }
}
