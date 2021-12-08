using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CMS.Repositories
{
    public class Repository : IRepository
    {
        private readonly AppDbContext context;

        public Repository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Create<T>(T entity) where T : IEntity
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task Update<T>(T entity) where T : class, IEntity
        {
            T currentEntity = context.Set<T>().FirstOrDefault(x => x.Id == entity.Id);
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name != "Id")
                {
                    property.SetValue(currentEntity, property.GetValue(entity));
                }
            }

            context.Update(currentEntity);
            await context.SaveChangesAsync();
        }

        public async Task Delete<T>(T entity) where T : class, IEntity
        {
            T currentEntity = context.Set<T>().FirstOrDefault(x => x.Id == entity.Id);
            context.Remove(currentEntity);
            await context.SaveChangesAsync();
        }

        public IQueryable<T> Query<T>() where T : class, IEntity
            => context.Set<T>().AsNoTracking();
    }
}
