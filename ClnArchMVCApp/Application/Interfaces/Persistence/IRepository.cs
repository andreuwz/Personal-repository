namespace Application.Interfaces.Persistence
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll(); 
        T Get(int id);  
        void Add(T entity);
        void Delete(T entity);  
        void Update(T entity);  
    }
}
