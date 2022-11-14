using Catalogue.API.DTO.Response;

namespace Catalogue.API.Application.ShopProduct.Queries.GetProduct
{
    public interface IGetProduct
    {
        Task<GetProductModel> GetProductByIdAsync(Guid id);
    }
}