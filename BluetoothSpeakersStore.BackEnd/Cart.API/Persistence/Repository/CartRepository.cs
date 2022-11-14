using Microsoft.EntityFrameworkCore;
using Cart.API.Domain;

namespace Cart.API.Persistence.Repository
{
    internal class CartRepository : ICartRepository
    {
        private readonly DatabaseContext dbContext;

        public CartRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CreateCartAsync(ShoppingCart cart)
        {
            await dbContext.AddAsync(cart);
            await SaveChangesAsync();
            return true;    
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> RemoveCartAsync(ShoppingCart cart)
        {
            cart.Products.ForEach(product => dbContext.Products.Remove(product));
            await SaveChangesAsync();

            dbContext.Remove(cart);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            await dbContext.AddAsync(product);
            await SaveChangesAsync();
            return true;
        }

        public async Task<ShoppingCart> GetCartByIdAsync(Guid id)
        {
            return await dbContext.Carts.FirstOrDefaultAsync(prop => prop.CartId == id);
        }

        public async Task<ShoppingCart> GetCartByCreatorIdAsync(Guid id)
        {
            return await dbContext.Carts.FirstOrDefaultAsync(prop => prop.CreatorId == id);
        }

        public async Task<bool> AddProductToCartAsync(ShoppingCart cart, Product product)
        {
            cart.Products.Add(product);
            product.Carts.Add(cart);
         
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProductAsync(Product product)
        {
            dbContext.Products.Remove(product);
            
            await SaveChangesAsync();
            return true;
        }

        public Product GetProductByProductIdIfExistsInCart(ShoppingCart cart, Guid id)
        {
            return cart.Products.FirstOrDefault(prop => prop.ProductId == id);
        }

        public async Task<Product> GetProductByProductIdAsync(Guid productId)
        {
            return await dbContext.Products.FirstOrDefaultAsync(prop => prop.ProductId == productId);
        }

        public async Task<IEnumerable<Product>> GetProductsListByProductId(Guid productId)
        {
            var result = await dbContext.Products.Where(prop => prop.ProductId == productId).ToListAsync();
            return result;
        }
    }
}
