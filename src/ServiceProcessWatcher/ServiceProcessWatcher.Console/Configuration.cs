using System.Collections.Generic;

namespace ServiceProcessWatcher.Console
{
    public class Configuration
    {
        public List<string> Services { get; set; }
        public List<LogConfiguration> Logs { get; set; }

        public List<ETWConfiguration> Etw { get; set; }

        public List<EventLogConfiguration> EventLogs { get; set; }
    }

    public class ETWConfiguration
    {
        public string ProviderName { get; set; }
    }

    public class EventLogConfiguration
    {
        public string LogName { get; set; }
    }

    public class LogConfiguration
    {
        public string Type { get; set; }
        public string Location { get; set; }
    }


}
