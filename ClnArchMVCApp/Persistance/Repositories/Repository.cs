using Application.Interfaces.Persistence;
using Common.CustomExceptions;
using Domain.Common;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                context.Set<T>().Add(entity);
                Save();
            }
            catch (DbUpdateException e)
            {
                throw new UsernameNotUniqueException(e.Message, e);
            }
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
            try
            {
                context.Set<T>().Update(entity);
                Save();
            }
            
            catch (DbUpdateException e)
            {
                throw new UsernameNotUniqueException(e.Message, e);
            }
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
