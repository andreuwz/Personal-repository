using System.Text.Json;

namespace Cart.API.Common
{
    public static class AppConstants
    {
        public const string recievedEvent = "--> Event recieved !";
        public const string listeningToEvent = "--> Listening on the Message Bus...";
        public const string connectionShutDown = "--> Connection shutdown";
        public const string determiningEvent = "--> Determining event...";
        public const string eventDescriptionPublishedUser = "--> Event description - Information about logged user was published and collected";
        public const string eventDescriptionPublishedProduct = "--> Event description - Information about product was published and collected";
        public const string undeterminedEventType = "UndeturminedEvent";
        public const string eventTypePublishedLoggedUser = "PublishedLoggedUser";
        public const string eventTypeAddProductToCart = "AddProductToCart";
        public const string eventTypeloggedOut = "LogOut";
        public const string duplicateCartException = "User does not have a cart";
        public const string removeCart = "Shopping cart successfully removed";
        public const string removeCartOfLoggedUser = "Shopping cart of the logged user successfully removed";
        public const string unauthenticatedUser = "User was not authenticated!";
        public const string rabbitMQSentMessage = "--> RabbitMQ Message SENT";
        public const string rabbitMQNotSentMessage = "--> Rabbit MQ connection closed, not sending...";
        public const string eventTypePublishedProductForReturn = "ReturnedProducts";
        public const string loggedUserUnsufficientBalace = "The logged user balance cannot cover the cart sum!";
        public const string successfullCheckout = "You successfully bought the products in the shopping cart!";
        public const string eventTypeUpdatedUserInfo = "UpdatedUserInfo";
        public const string updatedUserInfo = "--> Event description - Information about updated user information was published and collected.";
        public const string eventTypeUserDelete = "UserDelete";
        public const string updatedProductInfo = "--> Event description - Information about updated product information was published and collected.";
        public const string eventTypeUpdatedProductInfo = "UpdatedProductInfo";
        public const string eventTypeDeletedProduct = "DeletedProduct";
        public const string productNotFound = "Product not found.";
        public const string cartNotFound = "Shopping cart not found.";
        public const string cartZeroCheckoutSum = "You cannot checkout cart without a total sum.";
        public const string listOfProductsNotFound = "There are no products with such Id.";
        public const string eventTypeSentDisapprovedProductQuantities = "ProductDisapprovedQuantities";
        public const string productActualQuantites = "--> Information about product actual quantities - published and recieved.";
        public const string productQuantitesNotSuffucient = "Cart checkout fail.";
        public const string eventTypeApprovedQuantities = "ApprovedQuantities";
        public const string eventTypeBuyProducts = "BuyProducts";
        public const string eventTypeUpdatedUserBalance = "UpdatedBalance";
        public const string productNotInTheCart = "The product is not available in the current cart.";
        public const string inCartProductDeleted = "The product was successfully removed from cart.";
        public const string rpcClientMessageSent = "[x] RPC Client - message sent.";
        public const string rpcClientResponseReceived = "[x] RPC Client - response received.";
        public const string circuitBreakerOpenMessage = "Please, wait for 5 minutes, before trying again.";

        public static string ProductQuantitesNotSuffucient(string name, int actualQuantity )
        {
            return $"The product {name} ordered quantites" +
                        $" are more than his available quantities: {actualQuantity}." +
                        $" Please, edit your cart.";
        }

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
