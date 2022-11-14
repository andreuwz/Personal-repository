using Cart.API.DTO.Request;

namespace Cart.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public interface IDeleteProductFromCart
    {
        Task DeleteProductFromCartAsync(PublishedProductModel productModel);
    }
}