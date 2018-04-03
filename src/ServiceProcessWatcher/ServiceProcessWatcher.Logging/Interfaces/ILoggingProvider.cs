using System;
using System.Threading.Tasks;

namespace ServiceProcessWatcher.Logging.Interfaces
{
    public interface ILoggingProvider<TInput>
    {
        Task LogInformationAsync(Func<TInput, Task> method, TInput input);
    }
}
