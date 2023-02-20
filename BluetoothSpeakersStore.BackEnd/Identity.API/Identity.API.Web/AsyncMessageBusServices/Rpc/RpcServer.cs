using Identity.API.Common;
using Identity.API.Web.AsyncMessageBusServices.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Identity.API.Web.AsyncMessageBusServices.Rpc
{
    public class RpcServer : BackgroundService
    {
        private readonly ConnectionState connectionState;
        private readonly IEventProcessor eventProcessor;
        public RpcServer(ConnectionState connectionState, IEventProcessor eventProcessor)
        {
            this.connectionState = connectionState;
            this.eventProcessor = eventProcessor;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            connectionState.Channel.QueueDeclare(queue: "userPublishQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            connectionState.Channel.QueuePurge("userPublishQueue"); //***Purge all messages in the queue
            connectionState.Channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(connectionState.Channel);
            connectionState.Channel.BasicConsume(queue: "userPublishQueue", autoAck: false, consumer: consumer);
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
                    var userBalance = await eventProcessor.ProcessRpcEventAsync(message); //*** Mandatory - await this process;
                    response = JsonSerializer.Serialize(userBalance); 
                    Console.WriteLine(AppConstants.rpcServerMessageProcessed);
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
