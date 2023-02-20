using Catalogue.API.Common;
using Catalogue.API.Web.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Catalogue.API.Web.AsyncMessageBusServices.Rpc
{
    public class RpcServer : BackgroundService
    {
        private readonly ConnectionState connectionState;
        private readonly SessionState sessionState;
        private readonly IEventProcessor eventProcessor;

        public RpcServer(ConnectionState connectionState, SessionState sessionState, IEventProcessor eventProcessor)
        {
            this.connectionState = connectionState;
            this.sessionState = sessionState;
            this.eventProcessor = eventProcessor;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            connectionState.Channel.QueueDeclare(queue: "productPublishQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            connectionState.Channel.QueuePurge("productPublishQueue"); //***Purge all messages in the queue
            connectionState.Channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(connectionState.Channel);
            connectionState.Channel.BasicConsume(queue: "productPublishQueue", autoAck: false, consumer: consumer);
            Console.WriteLine(AppConstants.rpcServerActive);

            consumer.Received += async (model, ea) =>
            {
                string response = null;

                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = connectionState.Channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(AppConstants.rpcServerMessageRecieved);
                    await eventProcessor.ProccessRpcEventAsync(message); //*** Mandatory - await this process;
                    Console.WriteLine(AppConstants.rpcServerMessageProcessed);
                    response = JsonSerializer.Serialize(sessionState.ProductEvaluationModel);
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                    response = "";
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    connectionState.Channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                    connectionState.Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    Console.WriteLine(AppConstants.rpcClientResponseSent);
                }
            };
        }
    }
}
