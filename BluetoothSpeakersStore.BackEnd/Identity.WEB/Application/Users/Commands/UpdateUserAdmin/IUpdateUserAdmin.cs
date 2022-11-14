using Identity.API.Domain;
using Identity.API.DTO.Request;

namespace Identity.API.Application.Users.Commands.UpdateUserAdmin
{
    public interface IUpdateUserAdmin
    {
        Task<bool> UpdateUserAsync(Guid id, AdminEditUserModel userModel);
    }
}