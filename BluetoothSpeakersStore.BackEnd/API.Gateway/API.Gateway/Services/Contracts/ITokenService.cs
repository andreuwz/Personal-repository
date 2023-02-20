using API.Gateway.Models;

namespace API.Gateway.Services.Contracts
{
    public interface ITokenService
    {
        void ApplyNewTokensToHttpHeaders(HttpContext context, TokenModel newTokens);
        Task<string> DecryptToken(HttpContext context);
        Task<string> ExtractTokenClaimValue(HttpContext context, string claimType);
        Task<TokenModel> IssueNewAccessTokenViaRefreshToken(HttpContext context, IHttpClientFactory httpClientFactory);
        Task<string> TrimAccessToken(string accessToken);
    }
}