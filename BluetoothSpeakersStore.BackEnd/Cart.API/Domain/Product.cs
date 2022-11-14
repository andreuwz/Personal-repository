using System.ComponentModel.DataAnnotations;

namespace Cart.API.Domain
{
    public class Product
    {
        public Product()
        {
            InCartId = new Guid();
            Carts = new List<ShoppingCart>();
        }
        public Guid InCartId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public string Comment { get; set; }
        public virtual List<ShoppingCart> Carts { get; set; }
    }
}
