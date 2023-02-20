using Microsoft.EntityFrameworkCore;
using Catalogue.API.Domain;

namespace Catalogue.API.Persistence.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext dbContext;

        public ProductRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CreateNewItemAsync(Product item)
        {
            await dbContext.Products.AddAsync(item);
            await SaveChangesAsync();
            return true;
        }


        public async Task<bool> RemoveItemAsync(Product item)
        {
            dbContext.Products.Remove(item);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateItemAsync(Product item)
        {
            dbContext.Products.Update(item);
            await SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllShoppingItemsAsync()
        {
            return await dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetShoppingProductAsync(Guid id)
        {
            return await dbContext.Products
                .FirstOrDefaultAsync(prop => prop.Id == id);
        }

        public async Task SaveChangesAsync()
        {
           await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCreatorId(string creatorId)
        {
            return await dbContext.Products.Where(prop => prop.CreatorId == creatorId).ToListAsync();
        }
    }
}
