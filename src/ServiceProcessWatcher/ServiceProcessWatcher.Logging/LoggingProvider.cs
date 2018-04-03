using ServiceProcessWatcher.Logging.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace ServiceProcessWatcher.Logging
{
    public class LoggingProvider
        : ILoggingProvider
    {
        private readonly TextWriter textWriter;

        public LoggingProvider(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public void LogInformation<TInput>(TInput input)
        {
            textWriter.WriteLine(input.ToString());
        }

        public async Task LogInformationAsync<TInput>(TInput input)
        {
            await textWriter.WriteLineAsync(input.ToString());
        }
    }
}