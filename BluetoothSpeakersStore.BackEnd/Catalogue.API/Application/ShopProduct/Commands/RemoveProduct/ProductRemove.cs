using AutoMapper;
using Catalogue.API.Application.ShopProduct.Validations;
using Catalogue.API.Common;
using Catalogue.API.Domain;
using Catalogue.API.DTO.Response;
using Catalogue.API.Persistence.Repository;
using Catalogue.API.Web.AsyncMessageBusServices.PublishMessages;
using System.Text.Json;

namespace Catalogue.API.Application.ShopProduct.Commands.RemoveProduct
{
    internal class ProductRemove : IProductRemove
    {
        private readonly IProductRepository repository;
        private readonly IProductValidations productValidations;
        private readonly IPublishNewMessage publishNewMessage;
        private readonly IMapper mapper;

        public ProductRemove(IProductRepository repository, IProductValidations productValidations, 
            IPublishNewMessage publishNewMessage, IMapper mapper)
        {
            this.repository = repository;
            this.productValidations = productValidations;
            this.publishNewMessage = publishNewMessage;
            this.mapper = mapper;
        }

        public async Task RemoveProductByIdAsync(Guid id)
        {
            await productValidations.EnsureProductExistsAsync(id);

            var product = await repository.GetShoppingProductAsync(id);
            PublishUpdatedProductData(product);

            await repository.RemoveItemAsync(product);
        }

        private void PublishUpdatedProductData(Product productForDelete)
        {
            var publishedUpdatedProduct = mapper.Map<PublishUpdatedProductModel>(productForDelete);
            publishedUpdatedProduct.EventType = AppConstants.eventTypeDeletedProduct;

            var message = JsonSerializer.Serialize(publishedUpdatedProduct);
            publishNewMessage.PublishMessage(message);
        }
    }
}
