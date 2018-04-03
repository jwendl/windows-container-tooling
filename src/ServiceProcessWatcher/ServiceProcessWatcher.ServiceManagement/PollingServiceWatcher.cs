using ServiceProcessWatcher.Logging.Interfaces;
using ServiceProcessWatcher.ServiceManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace ServiceProcessWatcher.ServiceManagement
{
    public class PollingServiceWatcher
        : IServiceWatcher
    {
        private readonly ILoggingProvider loggingProvider;

        public PollingServiceWatcher(ILoggingProvider loggingProvider)
        {
            this.loggingProvider = loggingProvider;
        }

        private const int PollInterval = 10 * 1000;

        public void StartServices(IEnumerable<string> serviceNames)
        {
            var services = new List<ServiceController>();
            foreach (var name in serviceNames)
            {
                var service = new ServiceController(name);
                services.Add(service);

                service.Start();
                Debug.WriteLine($"Started service {service}");
            }

            Task.Run(() => WatchServices(services));
        }

        private async Task WatchServices(IEnumerable<ServiceController> services)
        {
            while (true)
            {
                foreach (var service in services)
                {
                    service.Refresh();
                    switch (service.Status)
                    {
                        case ServiceControllerStatus.Running:
                            break;
                        default:
                            Debug.WriteLine($"{service.ServiceName} is not running");
                            loggingProvider.LogInformation($"{service.ServiceName} is not running");
                            throw new InvalidOperationException($"{service.ServiceName} is not running");
                    }
                }

                await Task.Delay(PollInterval);
            }
        }
    }
}
