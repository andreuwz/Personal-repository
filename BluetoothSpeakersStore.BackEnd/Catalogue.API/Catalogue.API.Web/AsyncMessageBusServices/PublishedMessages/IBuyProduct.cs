using Catalogue.API.DTO.Response;

namespace Catalogue.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public interface IBuyProduct
    {
        PublishProductToCartModel ProductQuantityEvaluationModel { get; }
        Task BuyProductAsync(SessionState sessionState);
    }
}