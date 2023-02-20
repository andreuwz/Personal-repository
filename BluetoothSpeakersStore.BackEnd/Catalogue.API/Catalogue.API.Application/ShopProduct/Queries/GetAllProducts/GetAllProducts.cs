using AutoMapper;
using Catalogue.API.DTO.Response;
using Catalogue.API.Persistence.Repository;

namespace Catalogue.API.Application.ShopProduct.Queries.GetAllProducts
{
    public class GetAllProducts : IGetAllProducts
    {
        private readonly IProductRepository itemRepository;
        private readonly IMapper mapper;

        public GetAllProducts(IProductRepository itemRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.itemRepository = itemRepository;
        }

        public async Task<IEnumerable<GetProductModel>> GetAllProductsAsync()
        {
            var products = await itemRepository.GetAllShoppingItemsAsync();
            var outputProducts = mapper.Map<IEnumerable<GetProductModel>>(products);

            return outputProducts;
        }
    }
}
