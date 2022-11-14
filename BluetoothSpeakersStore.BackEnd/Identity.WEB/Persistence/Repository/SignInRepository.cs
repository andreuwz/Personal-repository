using Identity.API.Domain;
using Identity.API.Persistence.RepositoryContract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Identity.API.Persistence.Repository
{
    internal class SignInRepository : SignInManager<User>, ISignInRepository
    {
        public SignInRepository(UserManager<User> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<User> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<User>> logger,
            IAuthenticationSchemeProvider schemes, IUserConfirmation<User> confirmation)

            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public async Task<SignInResult> LogInAsync(User user, string password)
        {
            return await base.PasswordSignInAsync(user, password, false, false);
        }

        public async Task LogOutAsync()
        {
           await base.SignOutAsync();
        }

        public bool IsUserSignedIn(ClaimsPrincipal user)
        {
            return base.IsSignedIn(user);
        }

        public async Task<SignInResult>IsLoginPossible(User user, string password)
        {
            return await base.CheckPasswordSignInAsync(user,password,false);
        }

    }
}



