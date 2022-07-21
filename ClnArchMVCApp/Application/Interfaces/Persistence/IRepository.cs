namespace Application.Interfaces.Persistence
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll(); 
        T Get(int id);  
        void Add(T entity);
        void Delete(T entity);  
        void Update(T entity);  
    }
}
