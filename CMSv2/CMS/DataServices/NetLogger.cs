using System;
using System.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain.Objects;
using Domain.Interfaces;

using Newtonsoft.Json;

namespace DataServices
{
    public class NetLogger : ILogger
    {
        private const string ErrorLevel = "error";
        private const string InfoLevel = "info";

        private readonly LoggerOptions _options;
        private readonly ResourceConnection _connection;
        private readonly string[] _levels = { ErrorLevel, InfoLevel };

        public NetLogger(ResourceConnection connection)
        {
            _connection = InitConnection(connection);
            _options = InitOptions(connection);
        }

        public LoggerOptions Options => _options;

        public async Task Error(string message)
        {
            await SendAsync(ErrorLevel, message);
        }

        public async Task Info(string message)
        {
            await SendAsync(InfoLevel, message);
        }

        private async Task SendAsync(string path, string message)
        {
            await Task.Delay(0);
            if (AllowSend(path))
            {
                new Thread(() => Send(path, message)).Start();
            }
        }

        private void Send(string path, string message)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.UploadString($"{_connection.Value}/{path}", 
                        JsonConvert.SerializeObject(new 
                        {
                            message = $"{_options.Get<string>("Origin")}, {message}" 
                        }));
                }
            }
            catch { }
        }

        private bool AllowSend(string level)
        {
            if (_options.Has("MinLevel"))
            {
                int currentLevelValue = Array.IndexOf(_levels, level);
                int allowedLevelValue = Array.IndexOf(_levels, _options.Get<string>("MinLevel"));

                return currentLevelValue <= allowedLevelValue;
            }
            else
            {
                return true;
            }
        }

        private LoggerOptions InitOptions(ResourceConnection connection)
        {
            LoggerOptions options = new LoggerOptions();

            string[] parts = connection.Value.Split(';');
            if (parts.Length > 0)
            {
                string minlevelValues = parts.FirstOrDefault(x => x.ToLower().Contains("minlevel="));
                if (minlevelValues != null)
                {
                    string minlevelValue = minlevelValues.Split('=').LastOrDefault()?.Trim();
                    if (_levels.Contains(minlevelValue))
                    {
                        options.Add("MinLevel", minlevelValue);
                    }
                }

                string originValues = parts.FirstOrDefault(x => x.ToLower().Contains("origin="));
                if (originValues != null)
                {
                    string originValue = originValues.Split('=').LastOrDefault()?.Trim();
                    if (!string.IsNullOrEmpty(originValue))
                    {
                        options.Add("Origin", originValue);
                    }
                }
            }

            return options;
        }

        private ResourceConnection InitConnection(ResourceConnection connection)
        {
            string[] parts = connection.Value.Split(';');

            return new ResourceConnection
            {
                Value = parts.First().Trim()
            };
        }
    }
}
