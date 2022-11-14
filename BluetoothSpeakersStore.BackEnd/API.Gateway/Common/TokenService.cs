using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace API.Gateway.Common
{
    public class TokenService
    {
        private readonly DataSecurity dataSecurity;
        private readonly IHttpClientFactory clientFactory;
        private readonly string refreshTokenUri = "https://localhost:7268/connect/token";

        public TokenService(DataSecurity dataSecurity, IHttpClientFactory clientFactory)
        {
            this.dataSecurity = dataSecurity;
            this.clientFactory = clientFactory;
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

        public async Task<TokenModel> IssueAccessTokenViaRefreshToken(string refreshToken)
        {
            var client = clientFactory.CreateClient();

            var tokenAccessParams = new List<KeyValuePair<string, string>>();
            tokenAccessParams.Add(new KeyValuePair<string, string>("client_id", "AngularClient"));
            tokenAccessParams.Add(new KeyValuePair<string, string>("client_secret", "Arhs!"));
            tokenAccessParams.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
            tokenAccessParams.Add(new KeyValuePair<string, string>("refresh_token", refreshToken));

            using (client)
            {
                var request = await client.PostAsync(refreshTokenUri, new FormUrlEncodedContent(tokenAccessParams));
                var response = await request.Content.ReadAsStringAsync();
                var outputTokens = JsonSerializer.Deserialize<TokenModel>(response);

                return outputTokens;
            }
        }

        public async Task GetNewAccessToken(HttpContext context)
        {
            //execute refresh token logic
            var encryptedRefreshToken = context.Request.Headers["RefreshAuth"].ToString();
            var decryptedRefreshToken = await dataSecurity.DecryptData(encryptedRefreshToken);

            var newTokens = await IssueAccessTokenViaRefreshToken(decryptedRefreshToken);

            ApplyNewTokensToHttpHeaders(context, newTokens);
        }
    }
}
