using ServiceProcessWatcher.EventLog.Interfaces;
using System;
using System.Diagnostics;
using DiagEventLog = System.Diagnostics.EventLog;

namespace ServiceProcessWatcher.EventLog
{
    public class EventLogWatcher
        : IEventLogWatcher
    {
        private readonly Action<string> output;
        private readonly string source;

        public EventLogWatcher(Action<string> output, string source)
        {
            this.output = output;
            this.source = source;
        }

        public void Watch(string logName)
        {
            var eventLog = new DiagEventLog(logName, ".", source)
            {
                EnableRaisingEvents = true,
            };
            eventLog.EntryWritten += EventLog_EntryWritten;
        }

        private void EventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            if (e.Entry.Source == source)
            {
                output(e.Entry.Message);
            }
        }
    }
}
