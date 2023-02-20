using Catalogue.API.Domain;
using Catalogue.API.DTO.Response;

namespace Catalogue.API.Web.AsyncMessageBusServices.PublishMessages
{
    public interface IPublishNewMessage
    {
        void PublishUpdatedProductData(Product productForUpdate);
        void PublishRemovedProductEvent(Product productForDelete);
        void PublishAddProductToCartEvent(PublishProductToCartModel addedProduct);
    }
}