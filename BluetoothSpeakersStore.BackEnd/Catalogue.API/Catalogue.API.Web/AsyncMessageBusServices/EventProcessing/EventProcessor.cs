using Catalogue.API.Common;
using Catalogue.API.DTO.Request;
using Catalogue.API.Web.AsyncMessageBusServices;
using Catalogue.API.Web.AsyncMessageBusServices.PublishedMessages;
using System.Text.Json;

namespace Catalogue.API.Web.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly SessionState sessionState;

        public EventProcessor(IServiceScopeFactory scopeFactory, SessionState sessionState)
        {
            this.scopeFactory = scopeFactory;
            this.sessionState = sessionState;
        }

        private string DetermineEvent(string message)
        {
            Console.WriteLine(AppConstants.determiningEvent);

            var eventType = JsonSerializer.Deserialize<EventTypeModel>(message);

            switch (eventType.EventType)
            {
                case "BuyProducts":
                    Console.WriteLine(AppConstants.productsForReturn);
                    return AppConstants.eventTypePublishedProductForBuy;
                case "UpdatedUserInfo":
                    Console.WriteLine(AppConstants.updatedUserInfo);
                    return AppConstants.eventTypeUpdatedUserInfo;
                default:
                    Console.WriteLine(AppConstants.undeterminedEventType);
                    return AppConstants.undeterminedEventType;
            }
        }

        public async Task ProcessEventAsync(string message) //Interface method
        {
            var newEvent = DetermineEvent(message);


            switch (newEvent)
            {
                case AppConstants.eventTypeUpdatedUserInfo:
                    using (var scope = scopeFactory.CreateScope())
                    {
                        var updateProductService = scope.ServiceProvider.GetRequiredService<IUpdatedUserInfo>();
                        var updatedUserInfo = JsonSerializer.Deserialize<PublishedUpdatedUserModel>(message);
                        await updateProductService.UpdateProductsCreatedByUpdatedUser(updatedUserInfo);
                    }
                    break;
            }
        }

        public async Task ProccessRpcEventAsync(string message)
        {
            var newEvent = DetermineEvent(message);

            switch(newEvent)
            {
                case AppConstants.eventTypePublishedProductForBuy:
                    using (var scope = scopeFactory.CreateScope())
                    {
                        var buyProductService = scope.ServiceProvider.GetRequiredService<IBuyProduct>();

                        var buyProductsList = JsonSerializer.Deserialize<PublishedListProductsForBuyModel>(message);
                        sessionState.ProductsListForBuy = buyProductsList.Products;
                        await buyProductService.BuyProductAsync(sessionState);
                        sessionState.ProductEvaluationModel = buyProductService.ProductQuantityEvaluationModel;
                    }

                    sessionState.FlushPublishedProductsForBuyData();
                    break;
            }
        }
    }
}

