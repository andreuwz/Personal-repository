namespace Catalogue.API.Application.ShopProduct.Commands.RemoveProduct
{
    public interface IProductRemove
    {
        Task RemoveProductByIdAsync(Guid id);
    }
}