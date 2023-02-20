using System.Text.Json;

namespace Catalogue.API.Common
{
    public static class AppConstants
    {
        public const string publishedLoggedUserEventType = "PublishedLoggedUser";
        public const string recievedEvent = "--> Event recieved !";
        public const string listeningToEvent = "--> Listening on the Message Bus...";
        public const string connectionShutDown = "--> Connection shutdown";
        public const string determiningEvent = "--> Determining event...";
        public const string eventDescriptionPublishedUser = "--> Event description - Information about logged user was published";
        public const string undeterminedEvent = "---> Could not determine event type...";
        public const string undeterminedEventType = "UndeturminedEvent";
        public const string loggedOutEventType = "LogOut";
        public const string productNotFound = "Product with such Id does not exist!";
        public const string createdProduct = "Product successfully created!";
        public const string updatedProduct = "Product successfully updated!";
        public const string removedProduct = "Product successfully removed!";
        public const string unauthenticatedUser = "User was not authenticated!";
        public const string rabbitMQSentMessage = "--> RabbitMQ Message SENT";
        public const string rabbitMQNotSentMessage = "--> Rabbit MQ connection closed, not sending...";
        public const string addProductToCartEvent = "AddProductToCart";
        public const string sentProduct = "Product sent to cart.";
        public const string unsufficientQuantity = "Product available quantity is less than what you ordered!";
        public const string eventTypeSentProduct = "AddProductToCart";
        public const string eventTypePublishedProductForBuy = "BuyProducts";
        public const string productsForReturn = "--> Event description - Information about returned products was published and collected.";
        public const string eventTypeUpdatedUserInfo = "UpdatedUserInfo";
        public const string updatedUserInfo = "--> Event description - Information about updated user information was published and collected.";
        public const string updatedProductInfo = "--> Event description - Information about updated product information was published and collected.";
        public const string eventTypeUpdatedProductInfo = "UpdatedProductInfo";
        public const string eventTypeDeletedProduct = "DeletedProduct";
        public const string userIsNotCreator = "User does not have products created by him.";
        public const string nullProductAfterReturn = "The product was deleted before ";
        public const string eventTypeSendDisapprovedProductQuantities = "ProductDisapprovedQuantities";
        public const string eventTypeApprovedQuantities = "ApprovedQuantities";
        public const string rpcServerMessageRecieved = "[x] RPC Server - message received.";
        public const string rpcServerMessageProcessed = "[x] RPC Server - message processed.";
        public const string rpcClientResponseSent = "[x] RPC Server - response to client - sent.";
        public const string rpcServerActive = " [x] Awaiting RPC requests";
        public const string negativeQuantity = "The entered quantity must be a positive number.";
        public const string infrastructureFailure = "One ore more public API components are not working properly!";
        public const string circuitBreakerOpenMessage = "Please, wait for 5 minutes, before trying again.";
        public static string RabbitMQTrySentMessage(int tryAttempts)
        {
            return $"--> RabbitMQ Initialize sending message ({tryAttempts})";
        }

        public static string RabbitMQInfrastructureError(int tryAttempts)
        {
            return $"--> System Communication Malfunction. Retry attempts: {tryAttempts}. " +
                $"Please, contact your Administrator immediately. Your information is not stored properly in the system.";
        }

        public static string SerializeSingleMessage(string message)
        {
            return JsonSerializer.Serialize(message);
        }

        public static string SerializeMultipleMessages(IEnumerable<string> messages)
        {
            return JsonSerializer.Serialize(messages);
        }
    }
}
