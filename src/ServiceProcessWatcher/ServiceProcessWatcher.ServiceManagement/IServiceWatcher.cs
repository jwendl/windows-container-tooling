using System;
using System.Collections.Generic;

namespace ServiceProcessWatcher.ServiceManagement
{
    public interface IServiceWatcher
    {
        void StartServices(IEnumerable<string> serviceNames, Action<string> callback);
    }
}
