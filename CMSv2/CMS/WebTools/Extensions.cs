using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace WebTools
{
    public static class Extensions
    {
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
