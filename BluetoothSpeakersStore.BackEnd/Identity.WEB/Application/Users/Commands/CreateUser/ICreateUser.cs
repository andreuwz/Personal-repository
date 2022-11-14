using Identity.API.DTO.Request;

namespace Identity.API.Application.Users.Commands.CreateUser
{
    public interface ICreateUser
    {
        Task<bool> CreateUserAsync(CreateUserModel userModel);
    }
}