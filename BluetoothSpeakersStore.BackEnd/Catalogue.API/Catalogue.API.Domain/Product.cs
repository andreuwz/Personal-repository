namespace Catalogue.API.Domain
{
    public class Product
    {
        public Product()
        {
            Id = new Guid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifierName { get; set; }

    }
}
