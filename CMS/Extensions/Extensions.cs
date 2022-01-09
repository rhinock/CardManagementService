using System;

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
                    if (dstProperty.Name == srcProperty.Name)
                    {
                        dstProperty.SetValue(result, srcProperty.GetValue(src));
                    }
                }
            }

            return result;
        }
    }
}
