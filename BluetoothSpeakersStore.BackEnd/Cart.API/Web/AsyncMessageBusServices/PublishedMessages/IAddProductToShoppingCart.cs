using Cart.API.Persistence.Repository;

namespace Cart.API.Web.AsyncMessageBusServices.PublishMessages
{
    public interface IAddProductToShoppingCart
    {
        Task AddProductToCartAsync(ICartRepository cartRepository);
    }
}