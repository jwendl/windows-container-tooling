using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace ServiceProcessWatcher.ServiceManagement
{
    public class PollingServiceWatcher : IServiceWatcher
    {
        private const int PollInterval = 10 * 1000;

        public void StartServices(IEnumerable<string> serviceNames, Action<string> callback)
        {
            var services = new List<ServiceController>();
            foreach (var name in serviceNames)
            {
                var service = new ServiceController(name);
                services.Add(service);

                if (service.Status != ServiceControllerStatus.Running)
                    service.Start();
                Debug.WriteLine($"Started service {service}");
            }

            Task.Run(() => WatchServices(services, callback));
        }

        private async Task WatchServices(IEnumerable<ServiceController> services, Action<string> callback)
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
                            callback($"{service.ServiceName} is not running");
                            break;
                    }
                }

                await Task.Delay(PollInterval);
            }
        }
    }
}
