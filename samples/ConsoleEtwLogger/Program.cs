using System.Diagnostics.Tracing;

[EventSource(Name = "Microsoft-Demos-MySource")]
class Logger : EventSource
{
    public void MyFirstEvent(string MyName, int MyId) { WriteEvent(1, MyName, MyId); }
    public void MySecondEvent(int MyId) { WriteEvent(2, MyId); }
    public static Logger Log = new Logger();
}

class Program
{
    static void Main(string[] args)
    {
        Logger.Log.MyFirstEvent("Hi", 1);
        Logger.Log.MySecondEvent(1);
    }
}