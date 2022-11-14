using RabbitMQ.Client;
using Cart.API.Common;
using System.Text;
using RabbitMQ.Client.Exceptions;
using Cart.API.Application.Exceptions;
using Cart.API.Web.TransientFaultsPolicies;
using Polly.CircuitBreaker;

namespace Cart.API.Web.AsyncMessageBusServices.PublishMessages
{
    internal class PublishNewMessage : IPublishNewMessage
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

        public void PublishMessage(string message)
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
    }
}
