using Identity.API.DTO.Request;
using Identity.API.Persistence.RepositoryContract;

namespace Identity.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public interface IUpdateUserBalance
    {
        Task<bool> UpdateUserBalanceAsync(PublishedUserBalanceModel userModel);
    }
}