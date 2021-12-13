using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Domain.Objects;
using Domain.Interfaces;

namespace FileDataStore
{
    public class FileLogger : ILogger
    {
        private readonly ResourceConnection _connection;
        private readonly LoggerOptions _options;
        private static ReaderWriterLock fileLock = new ReaderWriterLock();

        public FileLogger(ResourceConnection connection)
        {
            _options = new LoggerOptions();
            _connection = connection;
        }

        public int Timeout => Options.Has("Timeout") == true ? Options.Get<int>("Timeout") : int.MaxValue;
        public LoggerOptions Options => _options;

        public async Task Info(string message)
        {
            fileLock.AcquireWriterLock(Timeout);
            await File.AppendAllTextAsync(GetFilePath(), $"{DateTime.Now:HH:mm:ss.ms} [INFO]: {message}{Environment.NewLine}");
            fileLock.ReleaseWriterLock();
        }

        public async Task Error(string message)
        {
            fileLock.AcquireWriterLock(Timeout);
            await File.AppendAllTextAsync(GetFilePath(), $"{DateTime.Now:HH:mm:ss.ms} [ERROR]: {message}{Environment.NewLine}");
            fileLock.ReleaseWriterLock();
        }

        private string GetFilePath()
        {
            string path = $"{_connection.Value}/{DateTime.Now:dd.MM.yyyy}.txt";
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(_connection.Value);
            }
            return path;
        }
    }
}