using CMS.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.ResponseModels
{
    public class ResponseModelWithData<T> : ResponseModel
    {
        public T Data { get; set; }

        public override string ToString()
        {
            string result = $"{(int)Result} - {Result}: {Message}\n";

            if (Data is ILoggable loggable)
            {
                result += loggable.GetData();
            }
            else if (Data is IEnumerable<ILoggable> enumerable)
            {
                result += string.Join(Environment.NewLine, enumerable?.Select(e => e.GetData()));
            }
            else
            {
                result += Data?.ToString();
            }

            return result;
        }
    }
}
