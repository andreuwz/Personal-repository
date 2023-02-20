using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Catalogue.API.Common;
using Catalogue.API.Web.EventProcessing;
using System.Text;

namespace Catalogue.API.Web.AsyncMessageBusServices
{
    public class MessageBusSubscriber : BackgroundService //background service  
    {
        private readonly IConfiguration config;
        private readonly IEventProcessor eventProcessor;
        private readonly ConnectionState connectionState;
        private string queueName;

        public MessageBusSubscriber(IConfiguration config, IEventProcessor eventProcessor, ConnectionState connectionState)
        {
            this.config = config;
            this.eventProcessor = eventProcessor;
            this.connectionState = connectionState;
            InitializeRabbitMQ();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(connectionState.Channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine(AppConstants.recievedEvent);

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                eventProcessor.ProcessEventAsync(notificationMessage);
            };

            connectionState.Channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            
            return Task.CompletedTask;
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = config["RabbitMQHost"],
                Port = int.Parse(config["RabbitMQPort"])
            };

            connectionState.Connection = factory.CreateConnection();
            connectionState.Channel = connectionState.Connection.CreateModel();
            connectionState.Channel.ExchangeDeclare(exchange: config["exchange"],type: ExchangeType.Fanout);
            queueName = connectionState.Channel.QueueDeclare().QueueName;
            connectionState.Channel.QueueBind(queue: queueName, exchange: config["exchange"], routingKey: "");

            Console.WriteLine(AppConstants.listeningToEvent);

            connectionState.Connection.ConnectionShutdown -= RabbitMQ_ConnectionShutDown;
        }

        private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine(AppConstants.connectionShutDown);
        }

        public override void Dispose()
        {
            if (connectionState.Channel.IsOpen)
            {
                connectionState.Channel.Close();
                connectionState.Connection.Close();
            }

            base.Dispose();
        }
    }
}
