using AutoMapper;
using Catalogue.API.Application.ShopProduct.Validations;
using Catalogue.API.DTO.Response;
using Catalogue.API.Persistence.Repository;

namespace Catalogue.API.Application.ShopProduct.Queries.GetProduct
{
    internal class GetProduct : IGetProduct
    {
        private readonly IProductRepository itemRepository;
        private readonly IMapper mapper;
        private readonly IProductValidations productValidations;

        public GetProduct(IProductRepository itemRepository, IMapper mapper, IProductValidations productValidations)
        {
            this.itemRepository = itemRepository;
            this.mapper = mapper;
            this.productValidations = productValidations;
        }

        public async Task<GetProductModel> GetProductByIdAsync(Guid id)
        {
            await productValidations.EnsureProductExistsAsync(id);

            var product = await itemRepository.GetShoppingProductAsync(id);
            var outputProduct = mapper.Map<GetProductModel>(product);

            return outputProduct;
        }
    }
}
