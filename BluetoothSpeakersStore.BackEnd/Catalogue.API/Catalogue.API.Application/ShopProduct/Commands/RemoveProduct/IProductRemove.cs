using Catalogue.API.Domain;

namespace Catalogue.API.Application.ShopProduct.Commands.RemoveProduct
{
    public interface IProductRemove
    {
        Task<Product> RemoveProductByIdAsync(Guid id);
    }
}