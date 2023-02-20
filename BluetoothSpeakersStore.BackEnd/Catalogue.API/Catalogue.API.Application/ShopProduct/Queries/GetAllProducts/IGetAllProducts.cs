using Catalogue.API.DTO.Response;

namespace Catalogue.API.Application.ShopProduct.Queries.GetAllProducts
{
    public interface IGetAllProducts
    {
        Task<IEnumerable<GetProductModel>> GetAllProductsAsync();
    }
}