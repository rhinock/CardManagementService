using System;
using System.Collections.Generic;
using System.Linq;

namespace CMS.ResponseModels
{
    public class ResponseDataModel<T> : ResponseModel
    {
        public T Data { get; set; }

        public override string ToString()
        {
            string result = $"{base.ToString()}\n";

            if (Data is IEnumerable<object> objectEnumerable)
            {
                result += string.Join(Environment.NewLine, objectEnumerable.Select(o => o.ToString()));
            }
            else
            {
                result += Data?.ToString();
            }

            return result;
        }
    }
}
