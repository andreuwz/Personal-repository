using AutoMapper;
using Cart.API.Application.Validations;
using Cart.API.DTO.Response;
using Cart.API.Persistence.Repository;

namespace Cart.API.Application.Queries.GetCartProducts
{
    internal class GetShoppingCart : IGetShoppingCart
    {
        private readonly ICartRepository cartRepository;
        private readonly IMapper mapper;
        private readonly ICartValidations cartValidations;

        public GetShoppingCart(ICartRepository cartRepository, IMapper mapper, ICartValidations cartValidations)
        {
            this.cartRepository = cartRepository;
            this.mapper = mapper;
            this.cartValidations = cartValidations;
        }

        public async Task<IEnumerable<GetCartProductsModel>> GetCartByIdAsync(Guid id)
        {
            var cart = await cartRepository.GetCartByIdAsync(id);
            cartValidations.EnsureCartExists(cart);
            var outputCart = mapper.Map<List<GetCartProductsModel>>(cart.Products
                .Where(prod => prod.Comment == "In stock")
                    .ToList());

            return outputCart;
        }
    }
}
