using System.Collections.Generic;

namespace WebTools
{
    public class MiddlewareOptions
    {
        private readonly Dictionary<string, object> _content;

        public MiddlewareOptions(IEnumerable<KeyValuePair<string, object>> collection = null)
        {
            _content = collection != null ? 
                new Dictionary<string, object>(collection) 
                : new Dictionary<string, object>();
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