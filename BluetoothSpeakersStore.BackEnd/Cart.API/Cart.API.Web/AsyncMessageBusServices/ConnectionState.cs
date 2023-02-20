using RabbitMQ.Client;

namespace Cart.API.Web.AsyncMessageBusServices
{
    public class ConnectionState
    {
        public IConnection Connection { get; set; }
        public IModel Channel { get; set; }
    }
}
