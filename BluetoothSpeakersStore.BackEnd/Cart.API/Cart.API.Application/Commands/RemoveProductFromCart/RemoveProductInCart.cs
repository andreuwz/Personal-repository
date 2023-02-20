using Cart.API.Application.Validations;
using Cart.API.Persistence.Repository;
using System.Security.Claims;

namespace Cart.API.Application.Commands.RemoveProductFromCart
{
    public class RemoveProductInCart : IRemoveProductInCart
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartValidations cartValidations;

        public RemoveProductInCart(ICartRepository cartRepository, ICartValidations cartValidations)
        {
            this.cartRepository = cartRepository;
            this.cartValidations = cartValidations;
        }

        public async Task RemoveProductFromLoggedUserCartAsync(Guid productId, ClaimsPrincipal loggedUser)
        {
            var loggedUserIdClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == "Id");
            var loggedUserId = new Guid(loggedUserIdClaim.Value);

            await cartValidations.EnsureUserHasCartAsync(loggedUserId);
            var cart = await cartRepository.GetCartByCreatorIdAsync(loggedUserId);
            var product = await cartRepository.GetProductByProductIdAsync(productId);
            cartValidations.EnsureProductExists(product);
            cartValidations.EnsureProductIsInLoggedUserCart(cart, product);

            cart.TotalSum -= product.TotalPrice;
            await cartRepository.RemoveProductAsync(product);
        }
    }
}
