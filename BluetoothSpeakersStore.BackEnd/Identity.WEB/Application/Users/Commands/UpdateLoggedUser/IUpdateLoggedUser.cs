using Identity.API.DTO.Request;

namespace Identity.API.Application.Users.Commands.UpdateUser
{
    public interface IUpdateLoggedUser
    {
        Task<bool> UpdateUserAsync(string id, EditUserModel userModel);
    }
}