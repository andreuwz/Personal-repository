using API.Gateway.Common;

namespace API.Gateway.Middleware
{
    public class AccessTokenDecryptor
    {
        private readonly RequestDelegate _next;
        private readonly TokenService tokenService;

        public AccessTokenDecryptor(RequestDelegate next, TokenService tokenService)
        {
            _next = next;
            this.tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string decryptedAccessToken = await tokenService.DecryptToken(context);

            if (string.IsNullOrEmpty(decryptedAccessToken))
            {
                await _next(context);
                return;
            }

            context.Request.Headers.Remove("Authorization");
            context.Request.Headers.Add("Authorization", "Bearer " + decryptedAccessToken);

            await _next(context);
        }
    }
}
