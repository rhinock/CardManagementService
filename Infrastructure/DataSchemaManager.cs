using System;
using PgDataStore;

using Domain.Objects;
using Domain.Interfaces;

namespace Infrastructure
{
    public static class DataSchemaManager
    {
        public static IDataSchema GetDataSchema<T>(ResourceConnection connection) where T : IMigrationDataContext
        {
            Type contextType = typeof(T);
            Type dataSchemaType = typeof(DataSchema<>);
            Type dataSchemaTypeWithContext = dataSchemaType.MakeGenericType(contextType);

            return (IDataSchema)Activator.CreateInstance(dataSchemaTypeWithContext, connection);
        }

        public static IDataSchema DataSchema<T>(this ResourceConnection connection) where T : IMigrationDataContext
        {
            return GetDataSchema<T>(connection);
        }
    }
}