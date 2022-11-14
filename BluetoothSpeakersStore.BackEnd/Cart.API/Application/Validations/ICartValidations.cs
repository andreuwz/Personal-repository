using Cart.API.Domain;

namespace Cart.API.Application.Validations
{
    public interface ICartValidations
    {
        Task<bool> EnsureUserHasCartAsync(Guid id);
        bool EnsureUserBalanceCoversCartSum(double userBalace, double cartSum);
        bool EnsureProductExists(Product product);
        bool EnsureCartExists(ShoppingCart cart);
        bool EnsureCartHasPositiveTotalSum(ShoppingCart cart);
        bool EnsureListOfProductsExists(IEnumerable<Product> listOfProducts);
        bool EnsureProductIsInLoggedUserCart(ShoppingCart cart, Product product);
    }
}