using Catalogue.API.Domain;

namespace Catalogue.API.Persistence.Repository
{
    public interface IProductRepository
    {
        Task<bool> CreateNewItemAsync(Product item);
        Task<IEnumerable<Product>> GetAllShoppingItemsAsync();
        Task<Product> GetShoppingProductAsync(Guid id);
        Task<bool> RemoveItemAsync(Product item);
        Task SaveChangesAsync();
        Task <bool> UpdateItemAsync(Product item);
        Task<IEnumerable<Product>> GetProductsByCreatorId(string creatorId);
    }
}