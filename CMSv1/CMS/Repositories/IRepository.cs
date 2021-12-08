using System.Linq;
using System.Threading.Tasks;

namespace CMS.Repositories
{
    public interface IRepository
    {
        Task Create<T>(T entity) where T : IEntity;
        Task Update<T>(T entity) where T : class, IEntity;
        Task Delete<T>(T entity) where T : class, IEntity;
        IQueryable<T> Query<T>() where T : class, IEntity;
    }
}
