using System.IO;
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
    }
}
