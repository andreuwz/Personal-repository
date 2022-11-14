using Catalogue.API.DTO.Response;

namespace Catalogue.API.Web.AsyncMessageBusServices.PublishedMessages
{
    internal interface IBuyProduct
    {
        PublishProductToCartModel ProductQuantityEvaluationModel { get; }
        Task BuyProductAsync(SessionState sessionState);
    }
}