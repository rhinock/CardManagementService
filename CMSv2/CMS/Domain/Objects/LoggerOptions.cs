using System.Collections.Generic;

namespace Domain.Objects
{
    public class LoggerOptions
    {
        private readonly Dictionary<string, object> _content;
        public LoggerOptions()
        {
            _content = new Dictionary<string, object>();
        }

        public void Add(string name, object value)
        {
            _content.Add(name, value);
        }

        public T Get<T>(string name)
        {
            return (T)_content[name];
        }
    }
}
