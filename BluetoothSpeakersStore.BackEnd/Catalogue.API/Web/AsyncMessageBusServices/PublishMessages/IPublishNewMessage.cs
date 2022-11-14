namespace Catalogue.API.Web.AsyncMessageBusServices.PublishMessages
{
    public interface IPublishNewMessage
    {
        void PublishMessage(string message);
    }
}