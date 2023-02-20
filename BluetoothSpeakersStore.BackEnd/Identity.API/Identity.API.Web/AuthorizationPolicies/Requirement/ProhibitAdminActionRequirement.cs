using Identity.API.Application.Users.Queries.GetUser;
using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Web.AuthorizationPolicies.Requirement
{
    public class ProhibitAdminActionRequirement : IAuthorizationRequirement
    {
    }
}
