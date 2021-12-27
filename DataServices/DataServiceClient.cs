using System;
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

using DataServices.Tools;
using DataServices.Objects;

namespace DataServices
{
    public class DataServiceClient : IRepository
    {
        private readonly ResourceConnection _connection;
        private readonly NetClient _netClient;

        public DataServiceClient(ResourceConnection connection)
        {
            _connection = GetConnection(connection);
            _netClient = new NetClient(connection);
        }

        public async Task Create<T>(T item) where T : class, IDataObject
        {
            string path = await GetStorePath(item.SourceName);

            NetClientResult result = await _netClient.Send(path, "POST", JsonConvert.SerializeObject(item));
            PropertyInfo identityProperty = typeof(T).GetProperty(item.IdentityName);

            TypeConverter typeConverter = TypeDescriptor.GetConverter(identityProperty.PropertyType);
            object identityValue = typeConverter.ConvertFromString(result.Metadata["ObjectId"]);
            identityProperty.SetValue(item, identityValue);
        }

        public async Task Update<T>(T item) where T : class, IDataObject
        {
            string path = await GetStorePath(item.SourceName);
            await _netClient.Send($"{path}({GetIdentityItem(item)})", "PATCH", JsonConvert.SerializeObject(item));
        }

        public async Task Delete<T>(T item) where T : class, IDataObject
        {
            string path = await GetStorePath(item.SourceName);
            await _netClient.Send($"{path}({GetIdentityItem(item)})", "DELETE", "");
        }

        public async Task<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class, IDataObject
        {
            string sourceName = Activator.CreateInstance<T>().SourceName;
            string path = await GetStorePath(sourceName);

            Term term = CreateTerm(predicate);
            NetClientResult result = await _netClient.Get($"{path}?$filter={term}");

            return JsonConvert.DeserializeObject<ItemsSearchResult<T>>(result.Data)?.Value?.FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetMany<T>(Expression<Func<T, bool>> predicate) where T : class, IDataObject
        {
            string sourceName = Activator.CreateInstance<T>().SourceName;
            string path = await GetStorePath(sourceName);
            NetClientResult result;

            if (predicate != null)
            {
                Term term = CreateTerm<T>(predicate);
                result = await _netClient.Get($"{path}?$filter={term}");
            }
            else
            {
                result = await _netClient.Get(path);
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
            NetClientResult result = await _netClient.Get($"{_connection.Value}/{objectName}");
            RouteInfo routeInfo = JsonConvert.DeserializeObject<RouteInfo>(result.Data);

            return routeInfo.ResourceConnection;
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
    }
}
