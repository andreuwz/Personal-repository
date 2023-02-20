using Cart.API.Domain;
using Cart.API.Persistence.Repository;
using Cart.API.Web.AsyncMessageBusServices.PublishedMessages.Contracts;

namespace Cart.API.Web.AsyncMessageBusServices.PublishMessages
{
    public class AddProductToShoppingCart : IAddProductToShoppingCart
    {
        private readonly SessionState sessionState;

        public AddProductToShoppingCart(SessionState sessionState)
        {
            this.sessionState = sessionState;
        }

        public async Task AddProductToCartAsync(ICartRepository cartRepository)
        {
            var foundCart = await cartRepository.GetCartByCreatorIdAsync(sessionState.PublishedProductModel.UserId);

            if (foundCart == null)
            {
                await CreateNewLoggedUserCartAndAddNewProductAsync(cartRepository);
            }
            else
            {
                await AddNewProductToLoggedUserCartAsync(cartRepository, foundCart);
            }
        }

        private async Task AddNewProductToLoggedUserCartAsync(ICartRepository cartRepository, ShoppingCart foundCart)
        {
            var foundProduct = cartRepository.GetProductByProductIdIfExistsInCart(foundCart, sessionState.PublishedProductModel.Id);

            if (foundProduct == null)
            {
                var newProduct = new Product()
                {
                    ProductId = sessionState.PublishedProductModel.Id,
                    Price = sessionState.PublishedProductModel.Price,
                    Name = sessionState.PublishedProductModel.Name,
                    Quantity = sessionState.PublishedProductModel.Quantity,
                    Comment = "In stock"
                };

                newProduct.TotalPrice = newProduct.Quantity * newProduct.Price;
                foundCart.TotalSum += newProduct.TotalPrice;
                await cartRepository.AddProductAsync(newProduct);
                await cartRepository.AddProductToCartAsync(foundCart, newProduct);
                sessionState.FlushSentProductData();
            }
            else
            {
                foundProduct.Quantity += sessionState.PublishedProductModel.Quantity;
                foundProduct.TotalPrice = foundProduct.Price * foundProduct.Quantity;
                foundCart.TotalSum += foundProduct.Price * sessionState.PublishedProductModel.Quantity;
                await cartRepository.SaveChangesAsync();
                sessionState.FlushSentProductData();
            }
        }

        private async Task CreateNewLoggedUserCartAndAddNewProductAsync(ICartRepository cartRepository)
        {
            var newCart = new ShoppingCart()
            {
                CreatorId = sessionState.PublishedProductModel.UserId,
                CreatorName = sessionState.PublishedProductModel.UserName,
                CreatedAt = DateTime.Now.Date,
            };

            await cartRepository.CreateCartAsync(newCart);

            var newProduct = new Product()
            {
                ProductId = sessionState.PublishedProductModel.Id,
                Price = sessionState.PublishedProductModel.Price,
                Name = sessionState.PublishedProductModel.Name,
                Quantity = sessionState.PublishedProductModel.Quantity,
                Comment = "In stock"
            };

            newProduct.TotalPrice = newProduct.Price * newProduct.Quantity;
            newCart.TotalSum += newProduct.TotalPrice;
            await cartRepository.AddProductAsync(newProduct);
            await cartRepository.AddProductToCartAsync(newCart, newProduct);
            sessionState.FlushSentProductData();
        }
    }
}
