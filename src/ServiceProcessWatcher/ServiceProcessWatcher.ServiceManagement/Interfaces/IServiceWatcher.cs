using System.Collections.Generic;

namespace ServiceProcessWatcher.ServiceManagement.Interfaces
{
    public interface IServiceWatcher
    {
        void StartServices(IEnumerable<string> serviceNames);
    }
}
