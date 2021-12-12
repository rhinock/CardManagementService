using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static async Task<string> GetBodyAsStringAsync(this HttpRequest request)
        {
            string body;

            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();
            }

            return body;
        }
    }
}
