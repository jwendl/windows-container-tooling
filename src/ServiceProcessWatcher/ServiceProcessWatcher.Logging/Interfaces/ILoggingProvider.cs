using System.Threading.Tasks;

namespace ServiceProcessWatcher.Logging.Interfaces
{
    public interface ILoggingProvider
    {
        void LogInformation<TInput>(TInput input);

        Task LogInformationAsync<TInput>(TInput input);
    }
}
