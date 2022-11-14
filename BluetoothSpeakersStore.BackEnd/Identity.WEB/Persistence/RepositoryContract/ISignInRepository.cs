using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.API.Persistence.RepositoryContract
{
    internal interface ISignInRepository
    {
        Task<SignInResult> LogInAsync(User user, string password);
        Task LogOutAsync();
        bool IsUserSignedIn(ClaimsPrincipal user);
        Task<SignInResult> IsLoginPossible(User user, string password);
    }
}