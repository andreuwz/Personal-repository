using Identity.API.DTO.Response;

namespace Identity.API.Web.IdentityServer.Contracts
{
    public interface IAccessTokenIssuer
    {
        Task<TokenModel> IssueIdentityToken(string username, string password);
    }
}