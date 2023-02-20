using Cart.API.Application.Validations;
using Cart.API.Domain;
using Cart.API.DTO.Request;
using Cart.API.Persistence.Repository;
using Cart.API.Web.AsyncMessageBusServices.PublishedMessages.Contracts;

namespace Cart.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public class DeleteProductFromCart : IDeleteProductFromCart
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartValidations cartValidations;

        public DeleteProductFromCart(ICartRepository cartRepository, ICartValidations cartValidations)
        {
            this.cartRepository = cartRepository;
            this.cartValidations = cartValidations;
        }

        public async Task DeleteProductFromCartAsync(PublishedProductModel productModel)
        {
            var products = await cartRepository.GetProductsListByProductId(productModel.Id);
            cartValidations.EnsureListOfProductsExists(products);

            foreach (var product in products)
            {
                product.Carts.ForEach(cart =>
                {
                    cart.TotalSum -= product.TotalPrice;
                });

                product.Quantity = 0;
                product.TotalPrice = 0;
                product.Comment = "Product not available at the moment.";
            }

            await cartRepository.SaveChangesAsync();

            foreach (var product in products)
            {
                await cartRepository.RemoveProductAsync(product);
            }
        }
    }
}
