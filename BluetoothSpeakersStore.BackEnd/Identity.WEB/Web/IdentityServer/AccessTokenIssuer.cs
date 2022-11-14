using System.Text.Json;
using Identity.API.DTO.Response;
using Identity.API.Web.IdentityServer.Contracts;

namespace Identity.API.Web.IdentityServer
{
    internal class AccessTokenIssuer : IAccessTokenIssuer
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IDataSecurity dataSecurity;

        public AccessTokenIssuer(IHttpClientFactory clientFactory, IDataSecurity dataSecurity)
        {
            this.clientFactory = clientFactory;
            this.dataSecurity = dataSecurity;
        }

        public async Task<TokenModel> IssueIdentityToken(string username, string password)
        {
            var client = clientFactory.CreateClient();
            var getIdentityTokenUrl = "https://localhost:7268/connect/token";

            var tokenAccessParams = new List<KeyValuePair<string, string>>();

            tokenAccessParams.Add(new KeyValuePair<string, string>("grant_type", "password"));
            tokenAccessParams.Add(new KeyValuePair<string, string>("username", username));
            tokenAccessParams.Add(new KeyValuePair<string, string>("password", password));
            tokenAccessParams.Add(new KeyValuePair<string, string>("client_id", "AngularClient"));
            tokenAccessParams.Add(new KeyValuePair<string, string>("client_secret", "Arhs!"));
            tokenAccessParams.Add(new KeyValuePair<string, string>("scope", " offline_access Identity.API Catalogue.API Cart.API"));

            using (client)
            {
                var request = await client.PostAsync(getIdentityTokenUrl, new FormUrlEncodedContent(tokenAccessParams));
                var response = await request.Content.ReadAsStringAsync();

                return await GetFormattedAndSecuredJson(response);
            }
        }

        private async Task<TokenModel> GetFormattedAndSecuredJson(string content)
        {
            var token = JsonSerializer.Deserialize<TokenModel>(content);
            token.AccessToken = await dataSecurity.EncryptData(token.AccessToken);
            token.RefreshToken = await dataSecurity.EncryptData(token.RefreshToken);

            return token;
        }
    }
}
