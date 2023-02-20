using Duende.IdentityServer.Validation;
using Identity.API.Domain;
using Identity.API.Persistence.RepositoryContract;
using System.Security.Claims;

namespace WorkforceManagementAPI.WEB.IdentityAuth
{
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IIdentityRepository identityRepository;

        public PasswordValidator(IIdentityRepository identityRepository)
        {
            this.identityRepository = identityRepository;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            User user = await identityRepository.FindByNameAsync(context.UserName);
            List<Claim> userClaims = new List<Claim>();

            userClaims.Add(new Claim("Id", user.Id));
            context.Result = new GrantValidationResult(subject: user.Id, authenticationMethod: "password", claims: userClaims);
        }
    }
}