using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using Domain.Objects;
using Domain.Interfaces;

namespace PgDataStore
{
    public class Repository<TContext> : IRepository where TContext : DataContext, new()
    {
        private readonly ResourceConnection _connection;

        public Repository(ResourceConnection connection)
        {
            _connection = connection;
        }

        public async Task Create<T>(T item) where T : class, IDataObject
        {
            using (DataContext  context = GetContext())
            {
                await context.AddAsync(item);
                await context.SaveChangesAsync();
            }
        }

        public async Task Update<T>(T item) where T : class, IDataObject
        {
            using (DataContext  context = GetContext())
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                T currentItem = context.Set<T>().FirstOrDefault(GenerateItemExpression(item));

                foreach (PropertyInfo property in properties)
                {
                    if (property.Name != item.IdentityName)
                    {
                        property.SetValue(currentItem, property.GetValue(item));
                    }
                }

                context.Update(currentItem);
                await context.SaveChangesAsync();
            }
        }

        public async Task Delete<T>(T item) where T : class, IDataObject
        {
            using (DataContext  context = GetContext())
            {
                T currentItem = context.Set<T>().FirstOrDefault(GenerateItemExpression(item));

                context.Remove(currentItem);
                await context.SaveChangesAsync();
            }
        }

        public T Get<T>(Expression<Func<T, bool>> predicate) where T : class, IDataObject
        {
            using (DataContext  context = GetContext())
            {
                return context.Set<T>().AsNoTracking().FirstOrDefault(predicate);
            }
        }

        public IEnumerable<T> GetMany<T>(Expression<Func<T, bool>> predicate) where T : class, IDataObject
        {
            using (DataContext  context = GetContext())
            {
                return context.Set<T>($"{typeof(T).Name}s").AsNoTracking().Where(predicate).ToList();
            }
        }


        private Expression<Func<T, bool>> GenerateItemExpression<T>(T item) where T : class, IDataObject
        {
            PropertyInfo identityProperty = typeof(T).GetProperty(item.IdentityName);
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression member = Expression.Property(parameter, identityProperty.Name);

            BinaryExpression body = Expression.Equal(member, Expression.Constant(identityProperty.GetValue(item)));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private TContext GetContext()
        {
            return (TContext)Activator.CreateInstance(typeof(TContext), _connection.Value);
        }
    }
}
