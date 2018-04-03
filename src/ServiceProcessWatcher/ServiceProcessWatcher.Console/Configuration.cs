using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceProcessWatcher.Console
{
    public class Configuration
    {
        public List<string> Services { get; set; }
        public List<LogConfiguration> Logs { get; set; }

        public List<ETWConfiguration> Etw { get; set; }
    }

    public class ETWConfiguration
    {
        public string ProviderName { get; set; }
    }

    public class LogConfiguration
    {
        public string Type { get; set; }
        public string Location { get; set; }
    }


}
