using Catalogue.API.Application.Exceptions;
using Catalogue.API.Common;
using Catalogue.API.Domain;
using Catalogue.API.Persistence.Repository;

namespace Catalogue.API.Application.ShopProduct.Validations
{
    public class ProductValidations : IProductValidations
    {
        private readonly IProductRepository repository;

        public ProductValidations(IProductRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> EnsureProductExistsAsync(Guid id)
        {
            var product = await repository.GetShoppingProductAsync(id);

            var condition = product == null ? throw new ProductNotFoundException(AppConstants.productNotFound) : true;

            return condition;
        }

        public async Task<bool> EnsureEnteredQuantityIsAvailable(Guid id,int quantity)
        {
            await EnsureProductExistsAsync(id);
            var product = await repository.GetShoppingProductAsync(id);

            var condition = product.Quantity < quantity ? throw new UnsufficientProductQuantity(AppConstants.unsufficientQuantity) : true;

            return condition;
        }

        public bool EnsureUserHasProducts(IEnumerable<Product> userProducts)
        {
            return userProducts.Any() ? true : throw new ProductNotFoundException(AppConstants.userIsNotCreator);
        }

        public async Task<bool> EnsureProductHasQuantitiesAsync(Guid id, int buyQuantity)
        {
            var product = await repository.GetShoppingProductAsync(id);

            return product.Quantity >= buyQuantity ? true : false;
        }

        public bool EnsureProductQuantitiesArePositive(int quantity)
        {
            return quantity > 0 ? true : throw new UnsufficientProductQuantity(AppConstants.negativeQuantity);
        }
    }
}
