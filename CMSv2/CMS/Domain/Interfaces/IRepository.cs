using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IRepository
    {
        void Create<T>(T item) where T : class;
        void Update<T>(T item) where T : class;
        void Delete<T>(T item) where T : class;
        T Get<T>() where T : class;
        IEnumerable<T> GetMany<T>();
    }
}
