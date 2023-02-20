using API.Gateway.Services.Contracts;

namespace API.Gateway.Middleware
{
    public class AccessTokenRefresher
    {
        private const string ExpirationClaimName = "exp";
        
        private readonly RequestDelegate _next;
        private readonly ITokenService tokenService;
        private readonly IHttpClientFactory httpClientFactory;

        public AccessTokenRefresher(RequestDelegate next, ITokenService tokenService, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            this.tokenService = tokenService;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tokenExpirationString = await tokenService.ExtractTokenClaimValue(context, ExpirationClaimName);
            var tokenExpirationValue = 0l;

            if (string.IsNullOrWhiteSpace(tokenExpirationString) || !long.TryParse(tokenExpirationString, out tokenExpirationValue))
            {
                await _next(context);
                return;
            }

            var tokenExpirationTime = DateTimeOffset.FromUnixTimeSeconds(tokenExpirationValue).LocalDateTime;
            var currentTime = DateTime.Now;
            
            if (tokenExpirationTime <= currentTime)
            {
                var newTokens = await tokenService.IssueNewAccessTokenViaRefreshToken(context, httpClientFactory);
                tokenService.ApplyNewTokensToHttpHeaders(context, newTokens);

                await _next(context);
                return;
            }

            await _next(context);
        }
    }
}
