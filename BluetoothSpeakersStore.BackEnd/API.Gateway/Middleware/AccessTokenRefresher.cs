using API.Gateway.Common;

namespace API.Gateway.Middleware
{
    public class AccessTokenRefresher
    {
        private const string ExpirationClaimName = "exp";
        
        private readonly RequestDelegate _next;
        private readonly TokenService tokenService;

        public AccessTokenRefresher(RequestDelegate next,TokenService tokenService)
        {
            _next = next;
            this.tokenService = tokenService;
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
                await tokenService.GetNewAccessToken(context);
                await _next(context);
                return;
            }

            await _next(context);
        }
    }
}
