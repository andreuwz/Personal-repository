using AutoMapper;
using Catalogue.API.Application.ShopProduct.Validations;
using Catalogue.API.Common;
using Catalogue.API.DTO.Response;
using Catalogue.API.Persistence.Repository;
using Catalogue.API.Web.AsyncMessageBusServices.PublishMessages;
using System.Security.Claims;
using System.Text.Json;

namespace Catalogue.API.Application.ShopProduct.Commands.AddToCartProduct
{
    internal class AddProductToCart : IAddProductToCart
    {
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;
        private readonly IProductValidations productValidations;
        private readonly IPublishNewMessage sendProductToCart;

        public AddProductToCart(IMapper mapper, IProductRepository productRepository,
            IProductValidations productValidations, IPublishNewMessage sendProductToCart)
        {

            this.mapper = mapper;
            this.productRepository = productRepository;
            this.productValidations = productValidations;
            this.sendProductToCart = sendProductToCart;
        }

        public async Task<bool> AddProductToCartAsync(Guid id, int quantity, ClaimsPrincipal loggedUser)
        {
            Guid loggedUserId = ExtractLoggedUserId(loggedUser);
            var loggedUserUsername = ExtractLoggedUserUsername(loggedUser);

            await productValidations.EnsureEnteredQuantityIsAvailable(id, quantity);
            productValidations.EnsureProductQuantitiesArePositive(quantity);

            var product = await productRepository.GetShoppingProductAsync(id);
            var sendProduct = mapper.Map<PublishProductToCartModel>(product);

            sendProduct.Quantity = quantity;
            sendProduct.EventType = AppConstants.eventTypeSentProduct;
            sendProduct.UserId = loggedUserId;
            sendProduct.UserName = loggedUserUsername;

            var sendProductMessage = JsonSerializer.Serialize(sendProduct);
            sendProductToCart.PublishMessage(sendProductMessage);

            return true;
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
