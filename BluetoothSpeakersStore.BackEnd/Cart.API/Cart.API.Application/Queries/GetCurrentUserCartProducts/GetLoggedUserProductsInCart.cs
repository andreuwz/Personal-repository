using AutoMapper;
using Cart.API.Application.Validations;
using Cart.API.DTO.Response;
using Cart.API.Persistence.Repository;
using System.Security.Claims;

namespace Cart.API.Application.Queries.GetCurrentUserCartProducts
{
    public class GetLoggedUserProductsInCart : IGetLoggedUserProductsInCart
    {
        private readonly ICartRepository cartRepository;
        private readonly IMapper mapper;
        private readonly ICartValidations cartValidations;

        public GetLoggedUserProductsInCart(ICartRepository cartRepository, IMapper mapper,
            ICartValidations cartValidations)
        {
            this.cartRepository = cartRepository;
            this.mapper = mapper;
            this.cartValidations = cartValidations;
        }

        public async Task<IEnumerable<GetCartProductsModel>> GetLoggedUserCartProductsAsync(ClaimsPrincipal loggedUser)
        {
            var loggedUserIdClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == "Id");
            var loggedUserId = new Guid(loggedUserIdClaim.Value);

            await cartValidations.EnsureUserHasCartAsync(loggedUserId);
            var cart = await cartRepository.GetCartByCreatorIdAsync(loggedUserId);
            var outputProducts = mapper.Map<List<GetCartProductsModel>>(cart.Products.ToList());

            return outputProducts;
        }
    }
}
