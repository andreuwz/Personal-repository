using Identity.API.Domain;
using Identity.API.DTO.Request;
using System.Security.Claims;

namespace Identity.API.Application.Users.Validations
{
    public interface IUserValidations
    {
        bool EnsureUserExists(User user);
        Task<bool> EnsureCreatedUniqueUsernameAndEmailAsync(CreateUserModel userModel);
        Task<bool> EnsureEditedEmailSameAsUserModelAsync(User user, string email);
        Task<bool> EnsureEditedEmailIsUniqeAsync(string email);
        Task<bool> EnsureEditedUsernameIsUniqueAsync(string username);
        Task<bool> EnsureEditedUsernameSameAsUserModelAsync(User user, string username);

        Task<bool> EnsureUserNotInRoleAsync(string id, string role);
        Task<bool> EnsureUserInRoleAsync(string id, string role);
        bool EnsureUserNotSignedIn(ClaimsPrincipal user);
        Task<bool> EnsureRegisteredUniqueUsernameAndEmailAsync(RegisterUserModel userModel);
        Task<bool> EnsureLoginIsPossible(User user, string password);
        Task<bool> EnsureHttpAuthHeaderExists(string httpHeader);
    }
}