using Identity.API.Domain;
using Identity.API.Persistence.RepositoryContract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Identity.API.Persistence.Repository
{
    public class IdentityRepository : UserManager<User>, IIdentityRepository
    {
        public IdentityRepository(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, 
            IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base (store, optionsAccessor, passwordHasher, 
                userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }

        public async Task<IdentityResult> AddUserToRoleAsync(User user, string password)
        {
            return await base.AddToRoleAsync(user, password); 
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await CreateAsync(user, password);
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await base.DeleteAsync(user);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await Users.ToListAsync();
        }

        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            return (await GetRolesAsync(user)).ToList();
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            User user = await base.FindByIdAsync(userId);
            return await IsInRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> UpdateUserDataAsync(User user)
        {
            return await base.UpdateAsync(user);
        }

        public async Task<bool> ValidateUserCredentialsAsync(string userName, string password)
        {
            User user = await FindByNameAsync(userName);
            if (user != null)
            {
                bool result = await CheckPasswordAsync(user, password);
                return result;
            }
            return false;
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            return await base.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> ManualAddRoleToUserAsync(User user, string role)
        {
            return await base.AddToRoleAsync(user, role);   
        }

        public async Task<IdentityResult> ManualRemoveRoleFromUserAsync(User user, string role)
        {
            return await base.RemoveFromRoleAsync(user, role);
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            return await base.FindByNameAsync(userName);
        }
    }
}
