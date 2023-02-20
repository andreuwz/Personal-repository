namespace Catalogue.API.DTO.Response
{
    public class PublishUpdatedProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string EventType { get; set; }
        public int Quantity { get; set; }
    }
}
