using Cart.API.Application.Validations;
using Cart.API.Persistence.Repository;
using Cart.API.Web.AsyncMessageBusServices;

namespace Cart.API.Application.Commands.RemoveCart
{
    internal class RemoveShoppingCart : IRemoveShoppingCart
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartValidations cartValidations;

        public RemoveShoppingCart(ICartRepository cartRepository, ICartValidations cartValidations)
        {
            this.cartRepository = cartRepository;
            this.cartValidations = cartValidations;
        }

        public async Task RemoveCartByUserIdAsync(Guid id)
        {
            var cart = await cartRepository.GetCartByIdAsync(id);
            cartValidations.EnsureCartExists(cart);

            await cartRepository.RemoveCartAsync(cart);
        }
    }
}
