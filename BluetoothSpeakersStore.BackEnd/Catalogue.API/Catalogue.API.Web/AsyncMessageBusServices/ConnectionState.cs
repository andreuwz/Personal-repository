using RabbitMQ.Client;

namespace Catalogue.API.Web.AsyncMessageBusServices
{
    public class ConnectionState
    {
        public IConnection Connection { get; set; }
        public IModel Channel { get; set; }
    }
}
