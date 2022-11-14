using Cart.API.DTO.Request;

namespace Cart.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public interface IUpdateProductInfo
    {
        Task UpdateProductsInCartAsync(PublishedProductModel productModel);
    }
}