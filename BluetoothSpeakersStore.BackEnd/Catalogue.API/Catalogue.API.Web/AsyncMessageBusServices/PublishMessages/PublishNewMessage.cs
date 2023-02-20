using AutoMapper;
using Catalogue.API.Application.Exceptions;
using Catalogue.API.Common;
using Catalogue.API.Domain;
using Catalogue.API.DTO.Response;
using Catalogue.API.Web.TransientFaultsPolicies;
using Polly.CircuitBreaker;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Catalogue.API.Web.AsyncMessageBusServices.PublishMessages
{
    public class PublishNewMessage : IPublishNewMessage
    {
        private readonly ConnectionState connectionState;
        private readonly IConfiguration config;
        private readonly TransientFaultPolicy transientFaultPolicy;
        private readonly IMapper mapper;

        public PublishNewMessage(ConnectionState connectionState, IConfiguration config,
            TransientFaultPolicy transientFaultPolicy, IMapper mapper)
        {
            this.connectionState = connectionState;
            this.config = config;
            this.transientFaultPolicy = transientFaultPolicy;
            this.mapper = mapper;
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

        public void PublishUpdatedProductData(Product productForUpdate)
        {
            var publishedUpdatedProduct = mapper.Map<PublishUpdatedProductModel>(productForUpdate);
            publishedUpdatedProduct.EventType = AppConstants.eventTypeUpdatedProductInfo;

            var message = JsonSerializer.Serialize(publishedUpdatedProduct);
            PublishMessage(message);
        }

        public void PublishRemovedProductEvent(Product productForDelete)
        {
            var publishedUpdatedProduct = mapper.Map<PublishUpdatedProductModel>(productForDelete);
            publishedUpdatedProduct.EventType = AppConstants.eventTypeDeletedProduct;

            var message = JsonSerializer.Serialize(publishedUpdatedProduct);
            PublishMessage(message);
        }

        public void PublishAddProductToCartEvent(PublishProductToCartModel addedProduct)
        {
            var sendProductMessage = JsonSerializer.Serialize(addedProduct);
            PublishMessage(sendProductMessage);
        }
    }
}
