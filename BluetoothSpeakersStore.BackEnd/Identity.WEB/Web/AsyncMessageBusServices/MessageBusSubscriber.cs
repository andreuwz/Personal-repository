using Identity.API.Common.Constants;
using Identity.API.Web.AsyncMessageBusServices.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Identity.API.Web.AsyncMessageBusServices
{
    internal class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration config;
        private readonly ConnectionState connectionState;
        private readonly IEventProcessor eventProcessor;
        private string queueName;

        public MessageBusSubscriber(IConfiguration config, ConnectionState connectionState, IEventProcessor eventProcessor)
        {
            this.config = config;
            this.connectionState = connectionState;
            this.eventProcessor = eventProcessor;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = config["RabbitMQHost"],
                Port = int.Parse(config["RabbitMQPort"])
            };

            try
            {
                connectionState.Connection = factory.CreateConnection();
                connectionState.Channel = connectionState.Connection.CreateModel();
                connectionState.Channel.ExchangeDeclare(exchange: config["exchange"], type: ExchangeType.Fanout);
                queueName = connectionState.Channel.QueueDeclare().QueueName;
                connectionState.Channel.QueueBind(queue: queueName, exchange: config["exchange"], routingKey: "");
                connectionState.Connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine(AppConstants.rabbitMQConnected);

            }
            catch (Exception ex)

            {
                Console.WriteLine(AppConstants.rabbitMQNotConnected + ex.Message);
            }
        }


        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine(AppConstants.rabbitMQShutDown);
        }

        public void Dispose()
        {
            Console.WriteLine(AppConstants.rabbitMQDisposed);
            if (connectionState.Channel.IsOpen)
            {
                connectionState.Channel.Close();
                connectionState.Connection.Close();
            }
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
    }
}
