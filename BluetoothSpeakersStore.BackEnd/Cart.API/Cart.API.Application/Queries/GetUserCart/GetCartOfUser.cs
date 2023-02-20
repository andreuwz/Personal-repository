using AutoMapper;
using Cart.API.Application.Validations;
using Cart.API.DTO.Response;
using Cart.API.Persistence.Repository;

namespace Cart.API.Application.Queries.GetUserCart
{
    public class GetCartOfUser : IGetCartOfUser
    {
        private readonly ICartRepository cartRepository;
        private readonly IMapper mapper;
        private readonly ICartValidations cartValidations;
        public GetCartOfUser(ICartRepository cartRepository, IMapper mapper, ICartValidations cartValidations)
        {
            this.cartRepository = cartRepository;
            this.mapper = mapper;
            this.cartValidations = cartValidations;
        }

        public async Task<GetUserCartModel> GetCartByUserIdAsync(Guid userId)
        {
            await cartValidations.EnsureUserHasCartAsync(userId);
            
            var cart = await cartRepository.GetCartByCreatorIdAsync(userId);
            var outputCart = mapper.Map<GetUserCartModel>(cart);

            return outputCart;
        }
    }
}
