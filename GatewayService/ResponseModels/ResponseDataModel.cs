using System.Linq;
using System.Collections.Generic;

namespace GatewayService.ResponseModels
{
    public class ResponseDataModel<T> : ResponseModel
    {
        public T Data { get; set; }

        public override string ToString()
        {
            string result = $"{base.ToString().TrimEnd('}').TrimEnd(' ')}, Data: ";

            if (Data is IEnumerable<object> objectEnumerable)
            {
                result = $"[{string.Join(",", objectEnumerable.Select(o => o.ToString()))}] }}";
            }
            else
            {
                result = $"{Data?.ToString()} }}";
            }

            return result;
        }
    }
}
