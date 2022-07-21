using Application.Interfaces.Persistence;
using Domain.Common;

namespace Persistance.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly DatabaseContext context;

        public Repository(DatabaseContext context)
        {
            this.context = context;
        }

        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public T Get(int id)
        {
            return context.Set<T>()
                     .FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>();
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
    }
}
