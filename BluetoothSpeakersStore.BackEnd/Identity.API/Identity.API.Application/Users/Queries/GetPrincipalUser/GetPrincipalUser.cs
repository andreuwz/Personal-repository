using Identity.API.Domain;
using Identity.API.Persistence.RepositoryContract;
using System.Security.Claims;

namespace Identity.API.Application.Users.Queries.GetPrincipalUser
{
    public class GetPrincipalUser : IGetPrincipalUser
    {
        private readonly IIdentityRepository repository;

        public GetPrincipalUser(IIdentityRepository repository)
        {
            this.repository = repository;
        }

        public async Task<User> GetCurrentUserAsync(ClaimsPrincipal principal)
        {
            return await repository.GetUserAsync(principal);
        }
    }
}
