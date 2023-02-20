using Identity.API.DTO.Request;

namespace Identity.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public interface IUpdateUserBalance
    {
        Task<bool> UpdateUserBalanceAsync(PublishedUserBalanceModel userModel);
    }
}