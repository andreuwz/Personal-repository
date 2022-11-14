using Identity.API.DTO.Request;

namespace Identity.API.Application.Users.Commands.RegisterUser
{
    public interface IRegisterUser
    {
        Task<string> RegisterNewUserAsync(RegisterUserModel userModel);
    }
}