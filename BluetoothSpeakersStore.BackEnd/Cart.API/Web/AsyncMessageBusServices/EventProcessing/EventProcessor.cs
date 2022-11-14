using Cart.API.Common;
using Cart.API.DTO.Request;
using Cart.API.Persistence.Repository;
using Cart.API.Web.AsyncMessageBusServices.PublishedMessages;
using Cart.API.Web.AsyncMessageBusServices.PublishMessages;
using System.Text.Json;

namespace Cart.API.Web.AsyncMessageBusServices.EventProcessing
{
    internal class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly SessionState sessionState;
        private readonly IAddProductToShoppingCart addProductToShoppingCart;

        public EventProcessor(IServiceScopeFactory scopeFactory, SessionState sessionState, IAddProductToShoppingCart addProductToShoppingCart)
        {
            this.scopeFactory = scopeFactory;
            this.sessionState = sessionState;
            this.addProductToShoppingCart = addProductToShoppingCart;
        }

        private string DetermineEvent(string message)
        {
            Console.WriteLine(AppConstants.determiningEvent);

            var eventType = JsonSerializer.Deserialize<EventTypeModel>(message);

            switch (eventType.EventType)
            {
                case "AddProductToCart":
                    Console.WriteLine(AppConstants.eventDescriptionPublishedProduct);
                    return AppConstants.eventTypeAddProductToCart;
                case "UpdatedUserInfo":
                    Console.WriteLine(AppConstants.updatedUserInfo);
                    return AppConstants.eventTypeUpdatedUserInfo;
                case "UserDelete":
                    Console.WriteLine(AppConstants.updatedUserInfo);
                    return AppConstants.eventTypeUserDelete;
                case "UpdatedProductInfo":
                    Console.WriteLine(AppConstants.updatedProductInfo);
                    return AppConstants.eventTypeUpdatedProductInfo;
                case "DeletedProduct":
                    Console.WriteLine(AppConstants.updatedProductInfo);
                    return AppConstants.eventTypeDeletedProduct;
                default:
                    Console.WriteLine(AppConstants.undeterminedEventType);
                    return AppConstants.undeterminedEventType;
            }

        }

        public async Task ProcessEvent(string message) //Interface method
        {
            var newEvent = DetermineEvent(message);
            switch (newEvent)
            {
                case AppConstants.eventTypeAddProductToCart:

                    using (var scope = scopeFactory.CreateScope())
                    {
                        var repository = scope.ServiceProvider.GetRequiredService<ICartRepository>();
                        var sentProduct = JsonSerializer.Deserialize<PublishedProductModel>(message);
                        sessionState.PublishedProductModel = sentProduct;
                        await addProductToShoppingCart.AddProductToCartAsync(repository);
                    }
                    break;

                case AppConstants.eventTypeUpdatedUserInfo:

                    using (var scope = scopeFactory.CreateScope())
                    {
                        var updateUserInfoService = scope.ServiceProvider.GetRequiredService<IUpdatedUserInfo>();
                        var updatedUserInfo = JsonSerializer.Deserialize<PublishedUpdatedUserModel>(message);
                        await updateUserInfoService.UpdateCartFromUpdatedUserInfoAsync(updatedUserInfo);
                    }
                    break;

                case AppConstants.eventTypeUserDelete:
                    using (var scope = scopeFactory.CreateScope())
                    {
                        var deleteUserInfoService = scope.ServiceProvider.GetRequiredService<IDeleteUserInfo>();
                        var updatedUserInfo = JsonSerializer.Deserialize<PublishedUpdatedUserModel>(message);
                        await deleteUserInfoService.DeleteUserAndHisCartAsync(updatedUserInfo);
                    }
                    break;

                case AppConstants.eventTypeUpdatedProductInfo:
                    using (var scope = scopeFactory.CreateScope())
                    {
                        var updateProductService = scope.ServiceProvider.GetRequiredService<IUpdateProductInfo>();
                        var updatedProductInfo = JsonSerializer.Deserialize<PublishedProductModel>(message);
                        await updateProductService.UpdateProductsInCartAsync(updatedProductInfo);
                    }
                    break;

                case AppConstants.eventTypeDeletedProduct:
                    using (var scope = scopeFactory.CreateScope())
                    {
                        var deleteProductService = scope.ServiceProvider.GetRequiredService<IDeleteProductFromCart>();
                        var productForDelete = JsonSerializer.Deserialize<PublishedProductModel>(message);
                        await deleteProductService.DeleteProductFromCartAsync(productForDelete);
                    }
                    break;
            }
        }
    }
}
