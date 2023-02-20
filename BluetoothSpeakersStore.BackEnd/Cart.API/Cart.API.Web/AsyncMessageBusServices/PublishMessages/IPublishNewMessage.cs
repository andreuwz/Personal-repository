using Cart.API.Domain;

namespace Cart.API.Web.AsyncMessageBusServices.PublishMessages
{
    public interface IPublishNewMessage
    {
        void PublishUpdatedUserBalance(ShoppingCart loggedUserCart, Guid loggedUserId, double loggedUserBalance);
    }
}