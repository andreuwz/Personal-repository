using Catalogue.API.Domain;
using Catalogue.API.DTO.Response;

namespace Catalogue.API.Application.ShopProduct.Queries.GetProductAdmin
{
    public interface IGetProductAsAdmin
    {
        Task<GetProductModelAdmin> GetProductByIdAsync(Guid id);
    }
}