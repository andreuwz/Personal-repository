using AutoMapper;
using Catalogue.API.Application.ShopProduct.Validations;
using Catalogue.API.Common;
using Catalogue.API.DTO.Response;
using Catalogue.API.Persistence.Repository;
using System.Security.Claims;

namespace Catalogue.API.Application.ShopProduct.Commands.AddToCartProduct
{
    public class AddProductToCart : IAddProductToCart
    {
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;
        private readonly IProductValidations productValidations;

        public AddProductToCart(IMapper mapper, IProductRepository productRepository,
            IProductValidations productValidations)
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.productValidations = productValidations;
        }

        public async Task<PublishProductToCartModel> AddProductToCartAsync(Guid id, int quantity, ClaimsPrincipal loggedUser)
        {
            Guid loggedUserId = ExtractLoggedUserId(loggedUser);
            var loggedUserUsername = ExtractLoggedUserUsername(loggedUser);

            await productValidations.EnsureEnteredQuantityIsAvailable(id, quantity);
            productValidations.EnsureProductQuantitiesArePositive(quantity);

            var product = await productRepository.GetShoppingProductAsync(id);
            var productForSend = mapper.Map<PublishProductToCartModel>(product);

            productForSend.Quantity = quantity;
            productForSend.EventType = AppConstants.eventTypeSentProduct;
            productForSend.UserId = loggedUserId;
            productForSend.UserName = loggedUserUsername;

            return productForSend;
        }

        private string ExtractLoggedUserUsername(ClaimsPrincipal loggedUser)
        {
            var loggedUserUsernameClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
            return new string(loggedUserUsernameClaim.Value);
        }

        private Guid ExtractLoggedUserId(ClaimsPrincipal loggedUser)
        {
            var loggedUserIdClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == "Id");
            return new Guid(loggedUserIdClaim.Value);
        }
    }
}
