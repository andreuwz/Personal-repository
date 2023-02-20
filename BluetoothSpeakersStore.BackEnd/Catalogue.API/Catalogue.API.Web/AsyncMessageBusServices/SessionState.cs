using Catalogue.API.DTO.Request;
using Catalogue.API.DTO.Response;

namespace Catalogue.API.Web.AsyncMessageBusServices
{
    public class SessionState
    {
        private List<PublishedProductModel> productsListForBuy;
        private PublishProductToCartModel productEvaluationModel;

        public List<PublishedProductModel> ProductsListForBuy { get => productsListForBuy; set => productsListForBuy = value; }
        public PublishProductToCartModel ProductEvaluationModel { get => productEvaluationModel; set => productEvaluationModel = value; }

        public void FlushPublishedProductsForBuyData()
        {
            ProductsListForBuy = new List<PublishedProductModel>();
        }

        public void FlushProductEvaluationModel()
        {
            ProductEvaluationModel = new PublishProductToCartModel();
        }

        public PublishProductToCartModel CreateProductEvaluatedModel()
        {
            return new PublishProductToCartModel();
        }
    }
}

