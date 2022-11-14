using RabbitMQ.Client;

namespace Identity.API.Web.AsyncMessageBusServices
{
    internal class ConnectionState
    {
        public IConnection Connection { get; internal set; }
        public IModel Channel { get; internal set; }
    }
}
