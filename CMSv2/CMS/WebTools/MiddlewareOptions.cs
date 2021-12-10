using System.Collections.Generic;

namespace WebTools
{
    public class MiddlewareOptions
    {
        private Dictionary<string, object> _content;

        public MiddlewareOptions()
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
