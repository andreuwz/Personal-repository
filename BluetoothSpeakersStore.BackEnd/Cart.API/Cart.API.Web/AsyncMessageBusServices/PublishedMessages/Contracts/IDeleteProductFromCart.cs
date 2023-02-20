using Cart.API.DTO.Request;

namespace Cart.API.Web.AsyncMessageBusServices.PublishedMessages.Contracts
{
    public interface IDeleteProductFromCart
    {
        Task DeleteProductFromCartAsync(PublishedProductModel productModel);
    }
}