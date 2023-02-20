namespace Cart.API.DTO.Request
{
    public class PublishedProductModel
    {
        public Guid Id { get; set; } //Id from the Catalogue microservice
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string EventType { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
