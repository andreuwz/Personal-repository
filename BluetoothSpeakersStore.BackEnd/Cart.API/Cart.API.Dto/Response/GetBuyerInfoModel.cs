using Cart.API.Domain;

namespace Cart.API.Dto.Response
{
    public class GetBuyerInfoModel
    {
        public Guid BuyerId { get; set; }
        public ShoppingCart BuyerCart { get; set; }
    }
}
