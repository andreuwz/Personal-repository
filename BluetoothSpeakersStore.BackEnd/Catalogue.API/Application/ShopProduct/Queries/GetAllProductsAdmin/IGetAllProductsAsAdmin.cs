using Catalogue.API.DTO.Response;

namespace Catalogue.API.Application.ShopProduct.Queries.GetAllProductsAdmin
{
    public interface IGetAllProductsAsAdmin
    {
        Task<IEnumerable<GetProductModelAdmin>> GetAllProductsAsync();
    }
}