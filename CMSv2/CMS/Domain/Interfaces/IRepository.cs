using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IRepository
    {
        Task Create<T>(T item) where T : class, IDataObject;
        Task Update<T>(T item) where T : class, IDataObject;
        Task Delete<T>(T item) where T : class, IDataObject;
        T Get<T>(Expression<Func<T, bool>> predicate) where T : class, IDataObject;
        IEnumerable<T> GetMany<T>(Expression<Func<T, bool>> predicate) where T : class, IDataObject;
    }
}
