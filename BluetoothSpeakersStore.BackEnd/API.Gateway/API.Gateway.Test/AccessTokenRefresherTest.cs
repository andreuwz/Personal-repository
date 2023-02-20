namespace API.Gateway.Test
{
    public class AccessTokenRefresherTest : ServicesTestBase
    {
        [Fact]
        public async Task AccessToken_Refresher_Issues_New_Tokens_Successfully_Invokes_Next_Middleware_With_Expired_Token()
        {
            var accessTokenRefresher = SetupAccessTokenRefresherIssuingNewAccessToken();
            var httpContext = SetupCompleteHttpContext();
            
            var exception = Record.ExceptionAsync(async () => await accessTokenRefresher.InvokeAsync(httpContext));

            Assert.Null(await exception);
        }

        [Fact]
        public async Task AccessToken_Refresher_Invokes_Next_Middleware_When_Token_Null()
        {
            var accessTokenRefresher = SetupAccessTokenRefresher();
            var httpContext = SetupHttpContextClassNullToken();

            var exception = Record.ExceptionAsync(async () => await accessTokenRefresher.InvokeAsync(httpContext));

            Assert.Null(await exception);
        }

        [Fact]
        public async Task AccessToken_Refresher_Invokes_Next_Middleware_When_Token_EmptyString()
        {
            var accessTokenRefresher = SetupAccessTokenRefresher();
            var httpContext = SetupHttpContextClassEmptyToken();

            var exception = Record.ExceptionAsync(async () => await accessTokenRefresher.InvokeAsync(httpContext));

            Assert.Null(await exception);
        }
    }
}
