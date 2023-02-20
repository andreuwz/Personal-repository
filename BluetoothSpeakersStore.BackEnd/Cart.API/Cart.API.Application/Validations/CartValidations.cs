using Cart.API.Application.Exceptions;
using Cart.API.Common;
using Cart.API.Domain;
using Cart.API.Persistence.Repository;

namespace Cart.API.Application.Validations
{
    public class CartValidations : ICartValidations
    {
        private readonly ICartRepository cartRepository;

        public CartValidations(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        public async Task<bool> EnsureUserHasCartAsync(Guid id)
        {
            var cart = await cartRepository.GetCartByCreatorIdAsync(id);

            return  cart == null ? throw new UserDuplicateCartsException(AppConstants.duplicateCartException) : true;

        }

        public bool EnsureUserBalanceCoversCartSum(double userBalace, double cartSum)
        {
            return userBalace >= cartSum ? true : throw new UserUnsufficientBalance(AppConstants.loggedUserUnsufficientBalace);
        }

        public bool EnsureProductExists(Product product)
        {
            return product == null ? throw new ProductNotFoundException(AppConstants.productNotFound) : true;
        }

        public bool EnsureCartExists(ShoppingCart cart)
        {
            return cart == null ? throw new CartNotFoundException(AppConstants.cartNotFound) : true;
        }

        public bool EnsureCartHasPositiveTotalSum(ShoppingCart cart)
        {
            return cart.TotalSum > 0 ? true : throw new CartCheckoutSumException(AppConstants.cartZeroCheckoutSum);
        }

        public bool EnsureListOfProductsExists(IEnumerable<Product> listOfProducts)
        {
            return listOfProducts.Any() ? true : throw new ProductNotFoundException(AppConstants.listOfProductsNotFound);   
        }

        public bool EnsureProductIsInLoggedUserCart(ShoppingCart cart, Product product)
        {
            return cart.Products.Any(prod => prod.ProductId == product.ProductId) ? true : throw new ProductNotFoundException(AppConstants.productNotInTheCart);
        }
    }
}
