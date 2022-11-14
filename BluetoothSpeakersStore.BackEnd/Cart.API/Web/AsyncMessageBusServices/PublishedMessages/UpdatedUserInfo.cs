using Cart.API.Application.Validations;
using Cart.API.DTO.Request;
using Cart.API.Persistence.Repository;

namespace Cart.API.Web.AsyncMessageBusServices.PublishedMessages
{
    internal class UpdatedUserInfo : IUpdatedUserInfo
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartValidations cartValidations;

        public UpdatedUserInfo(ICartRepository cartRepository, ICartValidations cartValidations)
        {
            this.cartRepository = cartRepository;
            this.cartValidations = cartValidations;
        }

        public async Task UpdateCartFromUpdatedUserInfoAsync(PublishedUpdatedUserModel userModel)
        {
            var cart = await cartRepository.GetCartByCreatorIdAsync(new Guid(userModel.Id));
            cartValidations.EnsureCartExists(cart);

            cart.CreatorName = userModel.UserName;

            await cartRepository.SaveChangesAsync();
        }
    }
}
