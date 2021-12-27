using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Domain.Objects;
using Domain.Interfaces;

namespace FileDataStore
{
    public class FileLogger : ILogger
    {
        private readonly ResourceConnection _connection;
        private readonly LoggerOptions _options;
        private static ReaderWriterLock fileLock = new ReaderWriterLock();

        private static List<Log> _logs;
        private static bool _writingStarted;

        public FileLogger(ResourceConnection connection)
        {
            _options = new LoggerOptions();
            _connection = connection;
        }

        public int Timeout => Options.Has("Timeout") == true ? Options.Get<int>("Timeout") : int.MaxValue;
        public LoggerOptions Options => _options;


        public async Task Info(string message)
        {
            await AddLog("info", message);
        }

        public async Task Error(string message)
        {
            await AddLog("error", message);
        }


        private async Task AddLog(string type, string message)
        {
            if (!_writingStarted)
            {
                StartWriting();
            }
            lock (_logs)
            {
                _logs.Add(new Log
                {
                    Type = type,
                    Message = message,
                });
            }
            await Task.Delay(0);
        }

        private void StartWriting()
        {
            _logs = new List<Log>();
            _writingStarted = true;
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        lock (_logs)
                        {
                            _logs.ForEach(x => WriteLog(x));
                            _logs.Clear();
                        }
                    }
                    catch { }
                    Thread.Sleep(200);
                }
            }).Start();
        }

        private void WriteLog(Log log)
        {
            fileLock.AcquireWriterLock(Timeout);
            File.AppendAllText(GetFilePath(), $"{DateTime.Now:HH:mm:ss.ms} [{log.Type.ToUpper()}]: {log.Message}{Environment.NewLine}");
            fileLock.ReleaseWriterLock();
        }

        private string GetFilePath()
        {
            string path = $"{_connection.Value}/{DateTime.Now:dd.MM.yyyy}.txt";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(_connection.Value);
            }
            return path;
        }

        private class Log
        {
            public string Type { get; set; }

            public string Message { get; set; }
        }
    }
}