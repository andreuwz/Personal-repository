using API.Gateway.Models;
using API.Gateway.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;


namespace API.Gateway.Services
{
    public class TokenService : ITokenService
    {
        private readonly IDataSecurityService dataSecurity;
        private readonly string refreshTokenUri = "https://localhost:7254/connect/token";
        
        public TokenService(IDataSecurityService dataSecurity)
        {
            this.dataSecurity = dataSecurity;
        }

        public async Task<string> TrimAccessToken(string accessToken)
        {
            var wordForRemove = "Bearer ";

            return accessToken.Replace(wordForRemove, "");
        }

        public async Task<string> ExtractTokenClaimValue(HttpContext context, string claimType)
        {
            var token = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            JwtSecurityToken decodedAccessToken = await DecodeToken(token);

            return decodedAccessToken.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
        }

        private async Task<JwtSecurityToken> DecodeToken(string token)
        {
            var formattedToken = await TrimAccessToken(token);
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(formattedToken);
        }

        public void ApplyNewTokensToHttpHeaders(HttpContext context, TokenModel newTokens)
        {
            context.Request.Headers.Remove("Authorization");
            context.Request.Headers.Remove("RefreshAuth");

            context.Request.Headers.Add("Authorization", "Bearer " + newTokens.AccessToken);
            context.Request.Headers.Add("RefreshAuth", newTokens.RefreshToken);
        }


        public async Task<string> DecryptToken(HttpContext context)
        {
            var encryptedAccessToken = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(encryptedAccessToken))
            {
                return "";
            }

            var formattedAccessToken = await TrimAccessToken(encryptedAccessToken);
            var decryptedAccessToken = await dataSecurity.DecryptData(formattedAccessToken);

            return decryptedAccessToken;
        }

        public async Task<TokenModel> IssueNewAccessTokenViaRefreshToken(HttpContext context, IHttpClientFactory httpClientFactory)
        {
            //execute refresh token logic
            var encryptedRefreshToken = context.Request.Headers["RefreshAuth"].ToString();
            var decryptedRefreshToken = await dataSecurity.DecryptData(encryptedRefreshToken);

            var tokenAccessParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", "AngularClient"),
                new KeyValuePair<string, string>("client_secret", "Arhs!"),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", decryptedRefreshToken)
            };

            var httpClient = httpClientFactory.CreateClient();
            TokenModel outputTokens;

            using (httpClient)
            {
                var request = await httpClient.PostAsync(refreshTokenUri, new FormUrlEncodedContent(tokenAccessParams));

                if (request.IsSuccessStatusCode)
                {
                    var response = await request.Content.ReadAsStringAsync();
                    outputTokens = JsonSerializer.Deserialize<TokenModel>(response);
                    return outputTokens;
                }

                throw new NullReferenceException("Identity Server error. Check if server is reachable or if the token is valid.");
            }
        }
    }
}
