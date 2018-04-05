using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Trace.WriteLine("About Page from trace out");

            ViewBag.Message = "Your application description page.";

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
}