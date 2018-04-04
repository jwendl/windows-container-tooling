using System.Collections.Generic;

namespace ServiceProcessWatcher.Console
{
    public class Configuration
    {
        public List<string> Services { get; set; } = new List<string>();

        public List<LogConfiguration> Logs { get; set; } = new List<LogConfiguration>();

        public List<ETWConfiguration> Etw { get; set; } = new List<ETWConfiguration>();

        public List<EventLogConfiguration> EventLogs { get; set; } = new List<EventLogConfiguration>();
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
