using Cart.API.Application.Exceptions;
using Cart.API.Common;
using Cart.API.Domain;
using Cart.API.DTO.Response;
using Cart.API.Web.TransientFaultsPolicies;
using Polly.CircuitBreaker;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Cart.API.Web.AsyncMessageBusServices.PublishMessages
{
    public class PublishNewMessage : IPublishNewMessage
    {
        private readonly ConnectionState connectionState;
        private readonly IConfiguration config;
        private readonly TransientFaultPolicy transientFaultPolicy;

        public PublishNewMessage(ConnectionState connectionState, IConfiguration config, TransientFaultPolicy transientFaultPolicy)
        {
            this.connectionState = connectionState;
            this.config = config;
            this.transientFaultPolicy = transientFaultPolicy;
        }

        private void PublishMessage(string message)
        {
            var tryAttempts = 1;

            try
            {
                transientFaultPolicy.MessageBusCombinedPolicies.Execute(() => 
                {
                    Console.WriteLine(AppConstants.RabbitMQTrySentMessage(tryAttempts));
                    tryAttempts++;

                    var messageBody = Encoding.UTF8.GetBytes(message);
                    connectionState.Channel.BasicPublish(exchange: config["exchange"], routingKey: "", basicProperties: null, body: messageBody);

                    Console.WriteLine(AppConstants.rabbitMQSentMessage);
                });
            }
            catch (BrokenCircuitException ex)
            {
                throw new InfrastructureFailureException(AppConstants.circuitBreakerOpenMessage);
            }
            catch (Exception ex)
            {
                throw new InfrastructureFailureException(AppConstants.RabbitMQInfrastructureError(tryAttempts - 1));
            }
        }

        public void PublishUpdatedUserBalance(ShoppingCart loggedUserCart, Guid loggedUserId, double loggedUserBalance)
        {
            var updatedUserBalance = new PublishUpdatedUserBalance()
            {
                Id = loggedUserId,
                Balance = loggedUserBalance - loggedUserCart.TotalSum
            };

            var balanceMessage = JsonSerializer.Serialize(updatedUserBalance);
            PublishMessage(balanceMessage);
        }
    }
}
