using AutoMapper;
using Cart.API.Application.Validations;
using Cart.API.DTO.Response;
using Cart.API.Persistence.Repository;
using System.Security.Claims;

namespace Cart.API.Application.Queries.GetCurrentUserCart
{
    internal class GetLoggedUserCart : IGetLoggedUserCart
    {
        private readonly ICartRepository cartRepository;
        private readonly IMapper mapper;
        private readonly ICartValidations cartValidations;

        public GetLoggedUserCart(ICartRepository cartRepository
            , IMapper mapper, ICartValidations cartValidations)
        {
            this.cartRepository = cartRepository;
            this.mapper = mapper;
            this.cartValidations = cartValidations;
        }

        public async Task<GetUserCartModel> GetLoggedUserCartAsync(ClaimsPrincipal loggedUser)
        {
            var loggedUserIdClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == "Id");
            var loggedUserId = new Guid(loggedUserIdClaim.Value);

            await cartValidations.EnsureUserHasCartAsync(loggedUserId);

            var cart = await cartRepository.GetCartByCreatorIdAsync(loggedUserId);
            var outputCart = mapper.Map<GetUserCartModel>(cart);

            return outputCart;
        }
    }
}
