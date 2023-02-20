using Cart.API.Common;

namespace Cart.API.DTO.Response
{
    public class PublishProductForPurchaseModel
    {
        public PublishProductForPurchaseModel()
        {
            Id = new Guid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
