namespace Catalogue.API.DTO.Response
{
    public class PublishProductToCartModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string EventType { get; set; }
        public int Quantity { get; set; }
        public Guid UserId { get; set; } //the creator of the shopping cart
        public string UserName { get; set; }
    }
}
