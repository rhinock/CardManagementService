using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using ObjectTools.Attributes;

namespace ObjectTools
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

        public static void Set<TSrc, TDst>(this TSrc src, TDst dst) where TDst : new()
        {
            Type srcType = typeof(TSrc);
            Type dstType = typeof(TDst);

            foreach (var dstProperty in dstType.GetProperties())
            {
                foreach (var srcProperty in srcType.GetProperties())
                {
                    var attribute = srcProperty.GetCustomAttribute<IgnoreConvertAttribute>();

                    if (attribute == null && dstProperty.Name == srcProperty.Name)
                    {
                        object srcValue = srcProperty.GetValue(src);
                        object dstValue = dstProperty.GetValue(dst);

                        if (dstValue != null && srcValue != dstValue)
                        {
                            srcProperty.SetValue(src, dstValue);
                        }
                    }
                }
            }
        }

        public static Dictionary<string, object> AsDictionary<T>(this T src)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (var property in typeof(T).GetProperties())
            {
                result.Add(property.Name, property.GetValue(src));
            }

            return result;
        }
    }
}
