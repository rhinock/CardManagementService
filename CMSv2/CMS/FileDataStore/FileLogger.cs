using System;
using System.IO;
using System.Threading.Tasks;

using Domain.Objects;
using Domain.Interfaces;

namespace FileDataStore
{
    public class FileLogger : ILogger
    {
        private readonly ResourceConnection _connection;
        private readonly LoggerOptions _options;

        public FileLogger(ResourceConnection connection)
        {
            _connection = connection;
        }

        public LoggerOptions Options => _options;

        public async Task Info(string message)
        {
            await File.AppendAllTextAsync(GetFilePath(), $"{DateTime.Now:HH:mm:ss.ms} [INFO]: {message}{Environment.NewLine}");
        }

        public async Task Error(string message)
        {
            await File.AppendAllTextAsync(GetFilePath(), $"{DateTime.Now:HH:mm:ss.ms} [ERROR]: {message}{Environment.NewLine}");
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