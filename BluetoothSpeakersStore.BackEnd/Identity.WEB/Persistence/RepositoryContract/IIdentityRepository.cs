using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.API.Persistence.RepositoryContract
{
    internal interface IIdentityRepository
    {
        Task<IdentityResult> AddUserToRoleAsync(User user, string password);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> DeleteUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserAsync(ClaimsPrincipal principal);
        Task<List<string>> GetUserRolesAsync(User user);
        Task<bool> IsUserInRoleAsync(string userId, string roleName);
        Task<bool> ValidateUserCredentialsAsync(string userName, string password);
        Task<IdentityResult> UpdateUserDataAsync(User user);
        Task<User> FindByIdAsync(string id);
        Task<User> FindByNameAsync(string UserName);
        Task<User> FindUserByEmailAsync(string email);
        Task<IdentityResult> ManualAddRoleToUserAsync(User user, string role);
        Task<IdentityResult> ManualRemoveRoleFromUserAsync(User user, string role);
    }
}
