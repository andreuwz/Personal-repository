using Cart.API.DTO.Request;

namespace Cart.API.Web.AsyncMessageBusServices
{
    public class SessionState
    {
        private PublishedProductModel publishedProductModel;

        public PublishedProductModel PublishedProductModel { get => publishedProductModel; set => publishedProductModel = value; }

        public SessionState()
        {
            PublishedProductModel = new PublishedProductModel();
        }

        public void FlushSentProductData()
        {
            PublishedProductModel = new PublishedProductModel();
        }
    }
}
