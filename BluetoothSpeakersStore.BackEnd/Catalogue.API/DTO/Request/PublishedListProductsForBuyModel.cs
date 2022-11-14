namespace Catalogue.API.DTO.Request
{
    public class PublishedListProductsForBuyModel
    {
        public PublishedListProductsForBuyModel()
        {
            Products = new List<PublishedProductModel>();
        }
        public List<PublishedProductModel> Products { get; set; }
        public string EventType { get; set; }
    }
}
