using Cart.API.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace Cart.API.Web.AsyncMessageBusServices.Rpc
{
    internal class RpcClient
    {
        private const string QUEUE_NAME = "rpc_queue";
        private readonly string replyQueueName;
        private readonly ConnectionState connectionState;
        private readonly EventingBasicConsumer consumer;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper =
                    new ConcurrentDictionary<string, TaskCompletionSource<string>>();

        public RpcClient(ConnectionState connectionState)
        {
            this.connectionState = connectionState;
      

            // declare a server-named queue
            replyQueueName = connectionState.Channel.QueueDeclare(queue: "").QueueName;
            consumer = new EventingBasicConsumer(connectionState.Channel);
            consumer.Received += (model, ea) =>
            {
                if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out TaskCompletionSource<string> tcs))
                {
                    return;
                }
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                tcs.TrySetResult(response);
            };

            connectionState.Channel.BasicConsume(
              consumer: consumer,
              queue: replyQueueName,
              autoAck: true);
        }

        public Task<string> CallAsync(string message, CancellationToken cancellationToken = default(CancellationToken)) // default - empty canc token
        {
            IBasicProperties props = connectionState.Channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var tcs = new TaskCompletionSource<string>();
            callbackMapper.TryAdd(correlationId, tcs);

            connectionState.Channel.BasicPublish(
               exchange: "",
               routingKey: QUEUE_NAME,
               basicProperties: props,
               body: messageBytes);

            cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out var tmp)); //delegate is called when the token is canceled
            Console.WriteLine(AppConstants.rpcClientMessageSent);
            return tcs.Task;
        }
    }
}
