using System;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.IIS_Trace;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;

namespace ServiceProcessWatcher.ETW
{
    public class EtwWatcher : IDisposable
    {
        private readonly Action<string> output;
        private readonly TraceEventSession session;

        public EtwWatcher(Action<string> output)
        {
            this.output = output;
            session = new TraceEventSession("ServiceProcessWatcher");
        }

        public void Watch(string providerName)
        {
            // Turn on the process events (includes starts and stops).
            session.EnableProvider(providerName);

            session.Source.Dynamic.All += traceEvent =>
            {
                output(traceEvent.ToString());
            };
        }

        public void Watch(string providerName, string eventName)
        {
            // Turn on the process events (includes starts and stops).
            session.EnableProvider(providerName);
            session.Source.Dynamic.AddCallbackForProviderEvent(providerName, eventName, traceEvent =>
            {
                output(traceEvent.ToString());
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
                // in a timely manner
                output("disposing of etw wacher");
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
