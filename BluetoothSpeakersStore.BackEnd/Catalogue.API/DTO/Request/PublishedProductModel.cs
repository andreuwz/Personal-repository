namespace Catalogue.API.DTO.Request
{
    public class PublishedProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
