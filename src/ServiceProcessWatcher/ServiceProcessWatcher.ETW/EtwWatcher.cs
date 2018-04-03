using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using ServiceProcessWatcher.ETW.Interfaces;
using ServiceProcessWatcher.Logging.Interfaces;
using System;

namespace ServiceProcessWatcher.ETW
{
    public class EtwWatcher
        : IEtwWatcher, IDisposable
    {
        private readonly TraceEventSession session;
        private readonly ILoggingProvider loggingProvider;

        public EtwWatcher(ILoggingProvider loggingProvider)
        {
            this.loggingProvider = loggingProvider;
            session = new TraceEventSession("ServiceProcessWatcher");
        }

        public void Watch(string providerName)
        {
            // Turn on the process events (includes starts and stops).
            session.EnableProvider(providerName);

            session.Source.Dynamic.All += traceEvent =>
            {
                loggingProvider.LogInformation(traceEvent);
            };
        }

        public void Watch(string providerName, string eventName)
        {
            // Turn on the process events (includes starts and stops).
            session.EnableProvider(providerName);
            session.Source.Dynamic.AddCallbackForProviderEvent(providerName, eventName, traceEvent =>
            {
                loggingProvider.LogInformation(traceEvent);
            });
        }

        public void StartListening()
        {
            // Listen (forever) for events till disposed is called.
            session.Source.Process();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // session.StopOnDispose = true; //default
                // By calling dispose the session will stop. 
                // this is the way to stop the realtime session
                // in a timely maner
                session?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~EtwWatcher()
        {
            Dispose(false);
        }
    }
}
