using Cart.API.Persistence.Repository;

namespace Cart.API.Web.AsyncMessageBusServices.PublishedMessages.Contracts
{
    public interface IAddProductToShoppingCart
    {
        Task AddProductToCartAsync(ICartRepository cartRepository);
    }
}