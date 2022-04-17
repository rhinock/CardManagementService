using Polly;

using System.Net;
using System.Linq;
using System.Threading.Tasks;

using Domain.Objects;

using DataServices.Objects;

namespace DataServices.Tools
{
    internal class NetClient
    {
        private readonly IAsyncPolicy _policy;

        public NetClient(ResourceConnection connection)
        {
            _policy = NetPolicies.GetPolicy(connection);
        }

        public async Task<NetClientResult> Send(string url, string method, string data)
        {
            return await _policy.ExecuteAsync(async () =>
            {
                using (WebClient client = new WebClient())
                {
                    string responseData = await client.UploadStringTaskAsync(url, method, data);
                    return new NetClientResult(responseData, client.ResponseHeaders.AllKeys.ToDictionary(x => x, x => client.ResponseHeaders[x]));
                }
            });
        }

        public async Task<NetClientResult> Get(string url)
        {
            return await _policy.ExecuteAsync(async () =>
            {
                using (WebClient client = new WebClient())
                {
                    string responseData = await client.DownloadStringTaskAsync(url);
                    return new NetClientResult(responseData, client.ResponseHeaders.AllKeys.ToDictionary(x => x, x => client.ResponseHeaders[x]));
                }
            });
        }
    }
}
