using Duende.IdentityServer.Models;
using System.Security.Claims;

namespace WorkforceManagementAPI.WEB.IdentityAuth
{
    public class IdentityConfig
    {
        public static IEnumerable<Client> Clients => new List<Client>
        {
           new Client
           {
            ClientId = "AngularClient",
            AccessTokenLifetime = 30,
            AllowOfflineAccess = true,
            AbsoluteRefreshTokenLifetime = 259200,
            RefreshTokenUsage = TokenUsage.ReUse,
            UpdateAccessTokenClaimsOnRefresh = true,
            AllowedCorsOrigins = new []
            {
                "http://localhost:4200",
                "https://localhost:7193"
            },
 
            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
 
            // secret for authentication
            ClientSecrets =
            {
                new Secret("Arhs!".Sha256())
            },
 
            // scopes that client has access to
            AllowedScopes = { "Identity.API", "offline_access", "Catalogue.API", "Roles", "Cart.API" }
           }
        };

        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResource("Roles", new[] { "Role" })
            };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
          {
                new ApiScope("Identity.API", "Identity.API", new string[]{"Id", ClaimTypes.Name, "Balance",ClaimTypes.Role, "MasterAdmin", "Administrator", "RegularUser"}),
                new ApiScope("offline_access", "RefreshToken"),
                new ApiScope("Catalogue.API", "Catalogue.API", new string[]{"Id", ClaimTypes.Name, "Balance",ClaimTypes.Role, "MasterAdmin", "Administrator", "RegularUser"}),
                new ApiScope("Cart.API","Cart.API", new string[]{"Id", ClaimTypes.Name, "Balance",ClaimTypes.Role, "MasterAdmin", "Administrator", "RegularUser"})
          };
    }
}