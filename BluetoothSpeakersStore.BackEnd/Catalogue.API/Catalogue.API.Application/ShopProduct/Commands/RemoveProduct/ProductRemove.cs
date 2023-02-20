using AutoMapper;
using Catalogue.API.Application.ShopProduct.Validations;
using Catalogue.API.Common;
using Catalogue.API.Domain;
using Catalogue.API.DTO.Response;
using Catalogue.API.Persistence.Repository;
using System.Text.Json;

namespace Catalogue.API.Application.ShopProduct.Commands.RemoveProduct
{
    public class ProductRemove : IProductRemove
    {
        private readonly IProductRepository repository;
        private readonly IProductValidations productValidations;
        private readonly IMapper mapper;

        public ProductRemove(IProductRepository repository, IProductValidations productValidations, 
             IMapper mapper)
        {
            this.repository = repository;
            this.productValidations = productValidations;
            this.mapper = mapper;
        }

        public async Task<Product> RemoveProductByIdAsync(Guid id)
        {
            await productValidations.EnsureProductExistsAsync(id);

            var product = await repository.GetShoppingProductAsync(id);
            await repository.RemoveItemAsync(product);

            return product;
        }
    }
}
