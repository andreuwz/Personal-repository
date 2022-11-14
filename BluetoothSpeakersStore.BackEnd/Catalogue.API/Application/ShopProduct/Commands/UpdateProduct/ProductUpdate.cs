using AutoMapper;
using Catalogue.API.Application.ShopProduct.Validations;
using Catalogue.API.Common;
using Catalogue.API.Domain;
using Catalogue.API.DTO.Request;
using Catalogue.API.DTO.Response;
using Catalogue.API.Persistence.Repository;
using Catalogue.API.Web.AsyncMessageBusServices.PublishMessages;
using System.Security.Claims;
using System.Text.Json;

namespace Catalogue.API.Application.ShopProduct.Commands.UpdateProduct
{
    internal class ProductUpdate : IProductUpdate
    {
        private readonly IMapper mapper;
        private readonly IProductRepository itemRepository;
        private readonly IProductValidations productValidations;
        private readonly IPublishNewMessage publishNewMessage;

        public ProductUpdate(IMapper mapper, IProductRepository itemRepository, IProductValidations productValidations, 
             IPublishNewMessage publishNewMessage)
        {
            this.mapper = mapper;
            this.itemRepository = itemRepository;
            this.productValidations = productValidations;
            this.publishNewMessage = publishNewMessage;
        }

        public async Task<bool> UpdateItemAsync(Guid id, UpdateProductModel productModel, ClaimsPrincipal loggedUser)
        {
            await productValidations.EnsureProductExistsAsync(id);
            var loggedUserUsernameClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);

            var productForUpdate = await itemRepository.GetShoppingProductAsync(id);

            mapper.Map(productModel, productForUpdate);
            productForUpdate.ModifierName = loggedUserUsernameClaim.Value;
            productForUpdate.ModifiedAt = DateTime.Now.Date;

            PublishUpdatedProductData(productForUpdate);
            await itemRepository.SaveChangesAsync();

            return true;
        }

        private void PublishUpdatedProductData(Product productForUpdate)
        {
            var publishedUpdatedProduct = mapper.Map<PublishUpdatedProductModel>(productForUpdate);
            publishedUpdatedProduct.EventType = AppConstants.eventTypeUpdatedProductInfo;

            var message = JsonSerializer.Serialize(publishedUpdatedProduct);
            publishNewMessage.PublishMessage(message);
        }
    }
}
