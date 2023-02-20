using Cart.API.Application.Validations;
using Cart.API.Persistence.Repository;
using System.Security.Claims;

namespace Cart.API.Application.Commands.RemoveLoggedUserCart
{
    public class RemoveLoggedUserShoppingCart : IRemoveLoggedUserShoppingCart
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartValidations cartValidations;

        public RemoveLoggedUserShoppingCart(ICartRepository cartRepository, ICartValidations cartValidations)
        {
            this.cartRepository = cartRepository;
            this.cartValidations = cartValidations;
        }

        public async Task RemoveCartOfLoggedUserasync(ClaimsPrincipal loggedUser)
        {
            var loggedUserIdClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == "Id");
            var loggedUserId = new Guid(loggedUserIdClaim.Value);

            await cartValidations.EnsureUserHasCartAsync(loggedUserId);

            var cart = await cartRepository.GetCartByCreatorIdAsync(loggedUserId);
            await cartRepository.RemoveCartAsync(cart);
        }
    }
}
