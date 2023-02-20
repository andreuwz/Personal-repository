using Identity.API.Application.Users.Validations;
using Identity.API.Persistence.RepositoryContract;
using System.Text.Json;

namespace Identity.API.Application.Users.Commands.LoginUser
{
    public class LoginUser : ILoginUser
    {
        private readonly IIdentityRepository identityRepository;
        private readonly IUserValidations userValidation;

        public LoginUser(IIdentityRepository identityRepository, IUserValidations userValidation)
        {
            this.identityRepository = identityRepository;
            this.userValidation = userValidation;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var loginUser = await identityRepository.FindByNameAsync(username);
            userValidation.EnsureUserExists(loginUser);
            await userValidation.EnsureLoginIsPossible(loginUser, password);

            var roleList = await identityRepository.GetUserRolesAsync(loginUser);
            var resultRoleList = JsonSerializer.Serialize(roleList);
            return resultRoleList;
        }
    }
}
