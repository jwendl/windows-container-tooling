using ServiceProcessWatcher.Logging.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ServiceProcessWatcher.Logging
{
    public class LoggingProvider<TInput>
        : ILoggingProvider<TInput>
    {
        private readonly TextWriter textWriter;

        public LoggingProvider(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public async Task LogInformationAsync(Func<TInput, Task> method, TInput input)
        {
            await textWriter.WriteLineAsync(input.ToString());
            await method(input);
        }
    }
}