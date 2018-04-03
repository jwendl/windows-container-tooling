namespace ServiceProcessWatcher.EventLog.Interfaces
{
    public interface IEventLogWatcher
    {
        void Watch(string logName);
    }
}
