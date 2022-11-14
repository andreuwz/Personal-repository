namespace Cart.API.Domain
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            CartId = new Guid();
            Products = new List<Product>();
        }
        public Guid CartId { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatorName { get; set; }
        public virtual List<Product> Products { get; set; }
        public double TotalSum { get; set; }
    }
}
