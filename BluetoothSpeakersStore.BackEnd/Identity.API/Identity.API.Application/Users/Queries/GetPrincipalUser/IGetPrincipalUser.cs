using Identity.API.Domain;
using System.Security.Claims;

namespace Identity.API.Application.Users.Queries.GetPrincipalUser
{
    public interface IGetPrincipalUser
    {
        Task<User> GetCurrentUserAsync(ClaimsPrincipal principal);
    }
}