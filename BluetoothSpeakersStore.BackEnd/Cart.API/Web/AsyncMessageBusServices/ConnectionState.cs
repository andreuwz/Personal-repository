using RabbitMQ.Client;

namespace Cart.API.Web.AsyncMessageBusServices
{
    internal class ConnectionState
    {
        public IConnection Connection { get; internal set; }
        public IModel Channel { get; internal set; }
    }
}
