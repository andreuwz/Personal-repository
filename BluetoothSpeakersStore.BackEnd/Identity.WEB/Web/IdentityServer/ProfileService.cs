using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Identity.API.Persistence.RepositoryContract;
using System.Security.Claims;

namespace Identity.API.Web.IdentityServer
{
    internal class ProfileService : IProfileService
    {
        private readonly IIdentityRepository identityRepository;

        public ProfileService(IIdentityRepository identityRepository)
        {
            this.identityRepository = identityRepository;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userIdClaim = context.Subject.Claims.FirstOrDefault(claim => claim.Type == "Id").Value;
            var user = await identityRepository.FindByIdAsync(userIdClaim);

            List<string> roles = await identityRepository.GetUserRolesAsync(user);
            List<Claim> userClaims = new List<Claim>();

            userClaims.Add(new Claim("Id", user.Id));
            userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            userClaims.Add(new Claim("Balance", user.Balance.ToString()));

            foreach (var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (roles.Contains("MasterAdmin"))
            {
                userClaims.Add(new Claim("MasterAdmin", "true"));
            }

            if (roles.Contains("Administrator"))
            {
                userClaims.Add(new Claim("Administrator", "true"));
            }

            if (roles.Contains("RegularUser"))
            {
                userClaims.Add(new Claim("RegularUser", "true"));
            }

            context.IssuedClaims.Clear();
            context.IssuedClaims.AddRange(userClaims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
    }
}
