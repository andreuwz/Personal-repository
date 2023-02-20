using Identity.API.Domain;

namespace Identity.API.Web.AsyncMessageBusServices.PublishMessages
{
    public interface IPublishNewMessage
    {
        void PublishDeletedUserDataMessageAsync(User userForUpdate);
        Task PublishUpdatedUserDataMessageAsync(User userForUpdate);
    }
}