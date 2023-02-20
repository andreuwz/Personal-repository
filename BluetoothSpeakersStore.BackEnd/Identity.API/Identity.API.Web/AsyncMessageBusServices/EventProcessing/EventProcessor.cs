using Identity.API.Common;
using Identity.API.DTO.Request;
using Identity.API.Web.AsyncMessageBusServices.PublishedMessages;
using System.Text.Json;

namespace Identity.API.Web.AsyncMessageBusServices.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory scopeFactory;
        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        private string DetermineEvent(string message)
        {
            Console.WriteLine(AppConstants.determiningEvent);

            var eventType = JsonSerializer.Deserialize<PublishedUserBalanceModel>(message);

            switch (eventType.EventType)
            {
                case "UpdatedBalance":
                    Console.WriteLine(AppConstants.updatedUserBalance);
                    return AppConstants.eventTypeUpdatedUserBalance;
                case "AcquireUserBalance":
                    Console.WriteLine(AppConstants.userInCheckout);
                    return AppConstants.eventAcquireUserBalance;
                default:
                    Console.WriteLine(AppConstants.undeterminedEventType);
                    return AppConstants.undeterminedEventType;
            }
        }

        public async Task ProcessEventAsync(string message) //Interface method
        {
            var newEvent = DetermineEvent(message);

            using (var scope = scopeFactory.CreateScope())
            {
                var buyService = scope.ServiceProvider.GetRequiredService<IUpdateUserBalance>();

                switch (newEvent)
                {
                    case AppConstants.eventTypeUpdatedUserBalance:
                        var updatedUserInfo = JsonSerializer.Deserialize<PublishedUserBalanceModel>(message);
                        await buyService.UpdateUserBalanceAsync(updatedUserInfo);
                        break;
                }
            }
        }

        public async Task<double> ProcessRpcEventAsync(string message) //Interface method
        {
            var newEvent = DetermineEvent(message);

            using (var scope = scopeFactory.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<ISendUserBalance>();

                switch (newEvent)
                {
                    case AppConstants.eventAcquireUserBalance:
                        var user = JsonSerializer.Deserialize<PublishedCheckoutUserInfo>(message);
                        return await userService.ExtractAndSendUserBalance(user.Id);
                }
                return 0;
            }
        }
    }
}
