using Polly;
using Polly.Retry;

using System;
using System.Net;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using ObjectTools;

using Newtonsoft.Json;

using Domain.Objects;
using Domain.Interfaces;

using DataServices.Objects;

namespace DataServices
{
    public class DataServiceClient : IRepository
    {
        private readonly ResourceConnection _connection;
        private readonly AsyncRetryPolicy _retryPolicy;

        public DataServiceClient(ResourceConnection connection)
        {
            _connection = GetConnection(connection);
            _retryPolicy = GetRetryPolicy(connection);
        }

        public async Task Create<T>(T item) where T : class, IDataObject
        {
            string path = await GetStorePath(item.SourceName);

            Result result = await Send(path, "POST", JsonConvert.SerializeObject(item));
            PropertyInfo identityProperty = typeof(T).GetProperty(item.IdentityName);

            TypeConverter typeConverter = TypeDescriptor.GetConverter(identityProperty.PropertyType);
            object identityValue = typeConverter.ConvertFromString(result.Metadata["ObjectId"]);
            identityProperty.SetValue(item, identityValue);
        }

        public async Task Update<T>(T item) where T : class, IDataObject
        {
            string path = await GetStorePath(item.SourceName);
            await Send($"{path}({GetIdentityItem(item)})", "PATCH", JsonConvert.SerializeObject(item));
        }

        public async Task Delete<T>(T item) where T : class, IDataObject
        {
            string path = await GetStorePath(item.SourceName);
            await Send($"{path}({GetIdentityItem(item)})", "DELETE", "");
        }

        public async Task<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class, IDataObject
        {
            string sourceName = Activator.CreateInstance<T>().SourceName;
            string path = await GetStorePath(sourceName);

            Term term = CreateTerm(predicate);
            Result result = await Get($"{path}?$filter={term}");

            return JsonConvert.DeserializeObject<ItemsSearchResult<T>>(result.Data)?.Value?.FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetMany<T>(Expression<Func<T, bool>> predicate) where T : class, IDataObject
        {
            string sourceName = Activator.CreateInstance<T>().SourceName;
            string path = await GetStorePath(sourceName);
            Result result;

            if (predicate != null)
            {
                Term term = CreateTerm<T>(predicate);
                result = await Get($"{path}?$filter={term}");
            }
            else
            {
                result = await Get(path);
            }

            return JsonConvert.DeserializeObject<ItemsSearchResult<T>>(result.Data)?.Value;
        }

        private Term CreateTerm<T>(Expression<Func<T, bool>> predicate)
        {
            return Term
                .Create(predicate)
                .Add(Term.EqualValue, "eq")
                .Add(Term.NotEqualValue, "ne")
                .Add(Term.AndValue, "and")
                .Add(Term.OrValue, "or")
                .Add(Term.GreaterThanValue, "gt")
                .Add(Term.LessThanValue, "lt")
                .Add(Term.GreaterThanOrEqualValue, "ge")
                .Add(Term.LessThanOrEqualValue, "le")
                .Add(Term.QuoteValue, "'");
        }

        private string GetIdentityItem<T>(T item) where T : IDataObject
        {
            PropertyInfo property = item.GetType().GetProperty(item.IdentityName);
            return property.GetValue(item)?.ToString();
        }

        private async Task<string> GetStorePath(string objectName)
        {
            Result result = await Get($"{_connection.Value}/{objectName}");
            RouteInfo routeInfo = JsonConvert.DeserializeObject<RouteInfo>(result.Data);

            return routeInfo.ResourceConnection;
        }

        private AsyncRetryPolicy GetRetryPolicy(ResourceConnection connection)
        {
            int retryCount = 0;
            int retryStart = 0;
            var connectionParts = connection.Value.Split(';').Select(x=>x.Trim());

            foreach (var part in connectionParts)
            {
                if (part.StartsWith("retryCount="))
                {
                    retryCount = int.Parse(part.Split('=').LastOrDefault());
                }
                if (part.StartsWith("retryStart="))
                {
                    retryStart = int.Parse(part.Split('=').LastOrDefault());
                }
            }

            return Policy
                .Handle<WebException>(x=> x.Response != null && x.Response is HttpWebResponse && (int)((HttpWebResponse)x.Response).StatusCode >= 500)
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryStart, retryAttempt)));
        }

        private ResourceConnection GetConnection(ResourceConnection connection)
        {
            string connectionValue = connection.Value.Split(';').First();

            return new ResourceConnection
            {
                Value = connectionValue,
                Type = connection.Type
            };
        }


        private async Task<Result> Send(string url, string method, string data)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                using (WebClient client = new WebClient())
                {
                    string responseData = await client.UploadStringTaskAsync(url, method, data);
                    return new Result(responseData, client.ResponseHeaders.AllKeys.ToDictionary(x => x, x => client.ResponseHeaders[x]));
                }
            });
        }

        private async Task<Result> Get(string url)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                using (WebClient client = new WebClient())
                {
                    string responseData = await client.DownloadStringTaskAsync(url);
                    return new Result(responseData, client.ResponseHeaders.AllKeys.ToDictionary(x => x, x => client.ResponseHeaders[x]));
                }
            });
        }

        private class Result
        {
            public Result(string data, Dictionary<string, string> metadata)
            {
                Data = data;
                Metadata = metadata;
            }

            public string Data { get; }

            public Dictionary<string, string> Metadata { get; }
        }
    }
}
