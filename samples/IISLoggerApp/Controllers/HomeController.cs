using System;
using System.Diagnostics;
using System.IO;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Logger.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //home page will trigger ETW iis logs if turned on in iis
            return View();
        }

        public ActionResult File()
        {
            Trace.WriteLine($"{DateTime.Now.ToLongTimeString()}: Trace message");

            ViewBag.FileName = "log.log";
            ViewBag.Message = "Write to file.";

            var path = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), $"{DateTime.Now.ToString("yyyy-MM-dd")}-Logs.txt");
            var fileInfo = new FileInfo(path);
            using (var streamWriter = new StreamWriter(fileInfo.FullName, true))
            {
                streamWriter.WriteLine($"This is a test {DateTime.Now}");
            }

            return View();
        }

        
        public ActionResult CustomEtw()
        {
            ViewBag.Message = "Custom ETW";

            ViewBag.CustomEventSource = Logger.SAMPLE_TESTSOURCE;
            Logger.Log.MyFirstEvent("Hi", 1);
            Logger.Log.MySecondEvent(1);

            return View();
        }

        public ActionResult LogEventLog()
        {
            var source = "TestSource";
            var eventLogMessage = "Writing to event log.";

            ViewBag.Message = "Logging eventlog";
            ViewBag.LogName = "Application";

            ViewBag.Source = source;
            ViewBag.EventLogMessage = eventLogMessage;


            // Create an EventLog instance and assign its source.
            EventLog myLog = new EventLog();
            myLog.Source = source;

            // Write an informational entry to the event log.    
            myLog.WriteEntry(eventLogMessage);

            return View();
        }
    }

    [EventSource(Name = SAMPLE_TESTSOURCE)]
    class Logger : EventSource
    {
        public void MyFirstEvent(string MyName, int MyId) { WriteEvent(1, MyName, MyId); }
        public void MySecondEvent(int MyId) { WriteEvent(2, MyId); }

        public const string SAMPLE_TESTSOURCE = "Sample-Demos-TestSource";
        public static Logger Log = new Logger();
    }
}