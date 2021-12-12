using System;
using System.Net;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using ObjectTools;

using Newtonsoft.Json;

using Domain.Objects;
using Domain.Interfaces;

using DataServices.Objects;
using System.ComponentModel;

namespace DataServices
{
    public class DataServiceClient : IRepository
    {
        private readonly ResourceConnection _connection;

        public DataServiceClient(ResourceConnection connection)
        {
            _connection = connection;
        }

        public async Task Create<T>(T item) where T : class, IDataObject
        {
            string path = await GetStorePath(item.SourceName);

            using (WebClient client = new WebClient())
            {
                await client.UploadStringTaskAsync(path, "POST", JsonConvert.SerializeObject(item));
                PropertyInfo identityProperty = typeof(T).GetProperty(item.IdentityName);

                TypeConverter typeConverter = TypeDescriptor.GetConverter(identityProperty.PropertyType);
                object identityValue = typeConverter.ConvertFromString(client.ResponseHeaders["ObjectId"]);
                identityProperty.SetValue(item, identityValue);
            }
        }

        public async Task Update<T>(T item) where T : class, IDataObject
        {
            string path = await GetStorePath(item.SourceName);

            using (WebClient client = new WebClient())
            {
                await client.UploadStringTaskAsync($"{path}({GetIdentityItem(item)})", "PATCH", JsonConvert.SerializeObject(item));
            }
        }

        public async Task Delete<T>(T item) where T : class, IDataObject
        {
            string path = await GetStorePath(item.SourceName);

            using (WebClient client = new WebClient())
            {
                await client.UploadStringTaskAsync($"{path}({GetIdentityItem(item)})", "DELETE", "");
            }
        }

        public async Task<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class, IDataObject
        {
            string sourceName = Activator.CreateInstance<T>().SourceName;
            string path = await GetStorePath(sourceName);
            Term term = CreateTerm<T>(predicate);

            using (WebClient client = new WebClient())
            {
                string responseData = await client.DownloadStringTaskAsync($"{path}?$filter={term}");
                ItemsSearchResult<T> items = JsonConvert.DeserializeObject<ItemsSearchResult<T>>(responseData);

                return items.Value.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<T>> GetMany<T>(Expression<Func<T, bool>> predicate) where T : class, IDataObject
        {
            string sourceName = Activator.CreateInstance<T>().SourceName;
            string path = await GetStorePath(sourceName);

            Term term;

            if (predicate != null)
                term = CreateTerm<T>(predicate);
            else
                term = null;

            using (WebClient client = new WebClient())
            {
                string responseData;

                if (term != null)
                    responseData = await client.DownloadStringTaskAsync($"{path}?$filter={term}");
                else
                    responseData = await client.DownloadStringTaskAsync($"{path}");

                ItemsSearchResult<T> items = JsonConvert.DeserializeObject<ItemsSearchResult<T>>(responseData);

                return items.Value;
            }
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
            using (WebClient client = new WebClient())
            {
                string responseData = await client.DownloadStringTaskAsync($"{_connection.Value}/{objectName}");
                RouteInfo routeInfo = JsonConvert.DeserializeObject<RouteInfo>(responseData);

                return routeInfo.ResourceConnection;
            }
        }
    }
}
