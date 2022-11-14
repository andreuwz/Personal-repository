using Cart.API.DTO.Request;

namespace Cart.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public interface IUpdatedUserInfo
    {
        Task UpdateCartFromUpdatedUserInfoAsync(PublishedUpdatedUserModel userModel);
    }
}