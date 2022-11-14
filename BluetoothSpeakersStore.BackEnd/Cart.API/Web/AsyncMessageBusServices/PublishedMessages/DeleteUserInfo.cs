using Cart.API.Application.Validations;
using Cart.API.DTO.Request;
using Cart.API.Persistence.Repository;

namespace Cart.API.Web.AsyncMessageBusServices.PublishedMessages
{
    internal class DeleteUserInfo : IDeleteUserInfo
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartValidations cartValidations;

        public DeleteUserInfo(ICartRepository cartRepository, ICartValidations cartValidations)
        {
            this.cartRepository = cartRepository;
            this.cartValidations = cartValidations;
        }

        public async Task DeleteUserAndHisCartAsync(PublishedUpdatedUserModel userModel)
        {
            var userCart = await cartRepository.GetCartByCreatorIdAsync(new Guid(userModel.Id));
            cartValidations.EnsureCartExists(userCart);

            await cartRepository.RemoveCartAsync(userCart);
            await cartRepository.SaveChangesAsync();
        }
    }
}
