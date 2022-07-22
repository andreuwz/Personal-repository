using Application.Interfaces.Persistence;
using Domain.Common;
using Domain.Users;

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
            Save();
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            Save();
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
            Save();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public User Login(string username, string password)
        {
            return context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
