using Identity.API.Application.Exceptions;
using Identity.API.Application.Users.Queries.GetUser;
using Identity.API.Common;
using Identity.API.Domain;
using Identity.API.Persistence.RepositoryContract;
using Identity.API.Web.AuthorizationPolicies.Requirement;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.API.Web.AuthorizationPolicies.Handler
{
    public class ProhibitAdminAccountActionHandler : AuthorizationHandler<ProhibitAdminActionRequirement>
    {
        private readonly IGetUser getUser;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IIdentityRepository repository;
        private readonly ISignInRepository signInRepository;

        public ProhibitAdminAccountActionHandler(IGetUser getUser, IHttpContextAccessor contextAccessor,
            IIdentityRepository repository, ISignInRepository signInRepository)
        {
            this.getUser = getUser;
            this.contextAccessor = contextAccessor;
            this.repository = repository;
            this.signInRepository = signInRepository;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProhibitAdminActionRequirement requirement)
        {
            string authHeader = contextAccessor.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader)) {
                context.Fail();
                await Task.CompletedTask;
            }

            var userId = await ExtractTokenIdClaim(authHeader);

            var foundUser = await getUser.GetUserByIdAsync(userId);
            var loggedUser = await GetLoggedUser(context.User);
            var isEditedUserAdmin = await EnsureAdministratorRole(foundUser);
            var isLoggedUserMasterAdmin = await EnsureMasterAdminRole(loggedUser);

            if (isEditedUserAdmin && !isLoggedUserMasterAdmin)
            {
                if (loggedUser.Id == foundUser.Id)
                {
                    context.Succeed(requirement);
                    await signInRepository.LogOutAsync();
                    await Task.CompletedTask;
                }
                else
                {
                    context.Fail();
                    await Task.CompletedTask;
                }
            }

            else if (isEditedUserAdmin && isLoggedUserMasterAdmin)
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }

            context.Succeed(requirement);
            await Task.CompletedTask;
        }

        private async Task<User> GetLoggedUser(ClaimsPrincipal user)
        {
            var loggedUser = await repository.GetUserAsync(user);
            var condition = loggedUser != null ? loggedUser : throw new ProhibitedAdminAccountActionException(AppConstants.notLoggedIn);

            return condition;
        }

        private async Task<bool> EnsureAdministratorRole(User user)
        {
            return await repository.IsUserInRoleAsync(user.Id, "Administrator");
        }

        private async Task<bool> EnsureMasterAdminRole(User user)
        {
            return await repository.IsUserInRoleAsync(user.Id, "MasterAdmin");
        }

        private async Task<string> ExtractTokenIdClaim(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();

            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var token = handler.ReadToken(authHeader) as JwtSecurityToken;

            return token.Claims.FirstOrDefault(claim => claim.Type == "Id").Value;
        }
    }
}
