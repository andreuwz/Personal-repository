using AutoMapper;
using Cart.API.Application.Validations;
using Cart.API.Dto.Response;
using Cart.API.Persistence.Repository;
using System.Security.Claims;

namespace Cart.API.Application.Commands.CheckoutCart
{
    public class CheckoutShoppingCart : ICheckoutShoppingCart
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartValidations cartValidations;

        public CheckoutShoppingCart(ICartRepository cartRepository,
            ICartValidations cartValidations, IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.cartValidations = cartValidations;
        }

        public async Task<GetBuyerInfoModel> GetBuyerInformation(ClaimsPrincipal loggedUser)
        {
            Guid loggedUserId;
            ExtractLoggedUserClaims(loggedUser, out loggedUserId);
            await cartValidations.EnsureUserHasCartAsync(loggedUserId);

            var loggedUserCart = await cartRepository.GetCartByCreatorIdAsync(loggedUserId);
            cartValidations.EnsureCartHasPositiveTotalSum(loggedUserCart);

            var buyerInfo = new GetBuyerInfoModel()
            {
                BuyerId = loggedUserId,
                BuyerCart = loggedUserCart
            };

            return buyerInfo;
        }

        private void ExtractLoggedUserClaims(ClaimsPrincipal loggedUser, out Guid loggedUserId)
        {
            var loggedUserIdClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == "Id");
            loggedUserId = new Guid(loggedUserIdClaim.Value);
        }
    }
}
