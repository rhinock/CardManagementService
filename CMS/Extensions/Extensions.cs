using CMS.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Reflection;

namespace CMS.Extensions
{
    public static class Extensions
    {
        public static TDst To<TSrc, TDst>(this TSrc src) where TDst : new()
        {
            Type srcType = typeof(TSrc);
            Type dstType = typeof(TDst);

            TDst result = new TDst();

            foreach (var dstProperty in dstType.GetProperties())
            {
                foreach (var srcProperty in srcType.GetProperties())
                {
                    var attribute = srcProperty.GetCustomAttribute<IgnoreConvertAttribute>();

                    if (attribute == null && dstProperty.Name == srcProperty.Name)
                    {
                        dstProperty.SetValue(result, srcProperty.GetValue(src));
                    }
                }
            }

            return result;
        }

        public static string GetErrorMessage(this ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary
                .Select(x => x.Value.Errors)
                .Where(x => x.Count > 0)
                .FirstOrDefault()?
                .FirstOrDefault()?
                .ErrorMessage;
        }
    }
}
