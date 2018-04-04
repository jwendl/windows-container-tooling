using ServiceProcessWatcher.LogFile.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ServiceProcessWatcher.LogFile
{
    public class LogFileWatcher
        : ILogFileWatcher
    {
        private readonly Action<string> output;

        public LogFileWatcher(Action<string> output)
        {
            this.output = output;
        }

        public void Watch(string path)
        {
            var fileSystemWatcher = new FileSystemWatcher(path);
            fileSystemWatcher.Created += FileSystemWatcher_Created;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
        }

        private async void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            var file = e.FullPath;
            await Task.Run(() => output(ReverseFileReader.Read(file, 1)));
        }

        private async void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            var file = e.FullPath;
            await Task.Run(() => output(ReverseFileReader.Read(file, 1)));
        }
    }
}
