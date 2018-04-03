using System;

namespace ServiceProcessWatcher.ETW.Interfaces
{
    public interface IEtwWatcher
        : IDisposable
    {
        void Watch(string providerName);

        void Watch(string providerName, string eventName);

        void StartListening();
    }
}
