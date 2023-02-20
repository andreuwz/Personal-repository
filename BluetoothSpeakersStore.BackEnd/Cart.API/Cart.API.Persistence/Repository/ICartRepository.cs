using Cart.API.Domain;

namespace Cart.API.Persistence.Repository
{
    public interface ICartRepository
    {
        Task<bool> CreateCartAsync(ShoppingCart cart);
        Task<bool> RemoveCartAsync(ShoppingCart cart);
        Task SaveChangesAsync();
        Task<ShoppingCart> GetCartByIdAsync(Guid id);
        Task<ShoppingCart> GetCartByCreatorIdAsync(Guid id);
        Task<bool> AddProductToCartAsync(ShoppingCart cart, Product product);
        Task<bool> AddProductAsync(Product product);
        Product GetProductByProductIdIfExistsInCart(ShoppingCart cart, Guid id);
        Task<Product> GetProductByProductIdAsync(Guid productId);
        Task<bool> RemoveProductAsync(Product product);
        Task<IEnumerable<Product>> GetProductsListByProductId(Guid productId);
    }
}