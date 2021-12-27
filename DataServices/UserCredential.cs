using Domain.Interfaces;
using Domain.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    public class UserCredential : IUser
    {
        private string _identity;
        private readonly ResourceConnection _connection;

        public UserCredential(ResourceConnection connection)
        {
            _connection = connection;
        }

        public string Identity => _identity;

        public bool IsAuth()
        {
            return !string.IsNullOrEmpty(Identity);
        }

        public async Task Load(string accessToken)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string data = await client.DownloadStringTaskAsync($"{_connection.Value}/cred?value={accessToken}");
                    _identity = JsonConvert.DeserializeObject<Dictionary<string, string>>(data).FirstOrDefault(x => x.Key.ToLower() == "identity").Value;
                }
            }
            catch { }
        }
    }
}
