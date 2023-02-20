using AutoMapper;
using Catalogue.API.DTO.Response;
using Catalogue.API.Persistence.Repository;

namespace Catalogue.API.Application.ShopProduct.Queries.GetAllProductsAdmin
{
    public class GetAllProductsAsAdmin : IGetAllProductsAsAdmin
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;

        public GetAllProductsAsAdmin(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GetProductModelAdmin>> GetAllProductsAsync()
        {
            var products = await repository.GetAllShoppingItemsAsync();
            var outputProducts = mapper.Map<IEnumerable<GetProductModelAdmin>>(products);

            return outputProducts;
        }
    }
}
