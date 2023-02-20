using Cart.API.Common;

namespace Cart.API.DTO.Response
{
    public class PublishListProductsForPurchaseModel
    {
        public PublishListProductsForPurchaseModel()
        {
            Products = new List<PublishProductForPurchaseModel>();
        }   

        public List<PublishProductForPurchaseModel> Products { get; set; }
        public string EventType { get; set; } = AppConstants.eventTypeBuyProducts;
    }
}
