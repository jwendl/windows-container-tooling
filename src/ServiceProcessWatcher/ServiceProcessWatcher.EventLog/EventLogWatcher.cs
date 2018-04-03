using ServiceProcessWatcher.EventLog.Interfaces;
using System;
using System.Diagnostics;
using System.Text;
using DiagEventLog = System.Diagnostics.EventLog;

namespace ServiceProcessWatcher.EventLog
{
    public class EventLogWatcher
        : IEventLogWatcher
    {
        private readonly Action<string> output;

        public EventLogWatcher(Action<string> output)
        {
            this.output = output;
        }

        public void Watch(string logName)
        {
            var eventLog = new DiagEventLog(logName);
            eventLog.EntryWritten += EventLog_EntryWritten;
        }

        private void EventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            var bytes = e.Entry.Data;
            var message = Encoding.UTF8.GetString(bytes);
            output(message);
        }
    }
}
