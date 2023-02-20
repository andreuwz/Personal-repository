using AutoMapper;
using Identity.API.Application.Exceptions;
using Identity.API.Common;
using Identity.API.Domain;
using Identity.API.DTO.Response;
using Identity.API.Persistence.RepositoryContract;
using Identity.API.Web.TransientFaultsPolicies;
using Polly.CircuitBreaker;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Identity.API.Web.AsyncMessageBusServices.PublishMessages
{
    public class PublishNewMessage : IPublishNewMessage
    {
        private readonly ConnectionState connectionState;
        private readonly IConfiguration config;
        private readonly TransientFaultPolicy transientFaultPolicy;
        private readonly IMapper mapper;
        private readonly IIdentityRepository repository;

        public PublishNewMessage(ConnectionState connectionState, IConfiguration config, TransientFaultPolicy transientFaultPolicy, 
            IMapper mapper, IIdentityRepository repository)
        {
            this.connectionState = connectionState;
            this.config = config;
            this.transientFaultPolicy = transientFaultPolicy;
            this.mapper = mapper;
            this.repository = repository;
        }

        public void PublishDeletedUserDataMessageAsync(User userForUpdate)
        {
            var publishUpdatedUser = mapper.Map<PublishUpdatedUserModel>(userForUpdate);
            publishUpdatedUser.EventType = AppConstants.eventTypeUserDelete;

            var updatedUserMasage = JsonSerializer.Serialize(publishUpdatedUser);
            PublishMessage(updatedUserMasage);
        }

        public async Task PublishUpdatedUserDataMessageAsync(User userForUpdate)
        {
            var publishUpdatedUser = mapper.Map<PublishUpdatedUserModel>(userForUpdate);
            publishUpdatedUser.EventType = AppConstants.eventTypeUpdatedUserInfo;
            publishUpdatedUser.Roles = await repository.GetUserRolesAsync(userForUpdate);

            var updatedUserMessage = JsonSerializer.Serialize(publishUpdatedUser);
            PublishMessage(updatedUserMessage);
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
    }
}
