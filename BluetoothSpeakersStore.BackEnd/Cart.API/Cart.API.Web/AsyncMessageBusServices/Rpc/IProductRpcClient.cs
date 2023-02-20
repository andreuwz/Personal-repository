using Cart.API.Domain;

namespace Cart.API.Web.AsyncMessageBusServices.Rpc
{
    public interface IProductRpcClient
    {
        Task<string> AcquireProductWantedQuantities(ShoppingCart loggedUserCart);
    }
}