using Catalogue.API.Domain;

namespace Catalogue.API.Application.ShopProduct.Validations
{
    public interface IProductValidations
    {
        Task<bool> EnsureProductExistsAsync(Guid id);
        Task<bool> EnsureEnteredQuantityIsAvailable(Guid id, int quantity);
        bool EnsureUserHasProducts(IEnumerable<Product> userProducts);
        Task<bool> EnsureProductHasQuantitiesAsync(Guid id, int buyQuantity);
        bool EnsureProductQuantitiesArePositive(int quantity);
    }
}