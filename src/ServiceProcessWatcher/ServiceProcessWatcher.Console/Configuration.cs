using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceProcessWatcher.Console
{
    public class Configuration
    {
        public List<string> Services { get; set; }
        public List<LogConfiguration> Logs { get; set; }
    }

    public class LogConfiguration
    {
        public string Type { get; set; }
        public string Location { get; set; }
    }
}
