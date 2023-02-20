namespace API.Gateway.Test
{
    public class AccessTokenDecryptorTest : ServicesTestBase
    {
        [Fact]
        public async Task AccessToken_Decryptor_With_Proper_Token_Successfully_Invokes_Next_Middleware()
        {
            var httpContext = SetupHttpContextClassWithEncryptedToken();
            var accessTokenDecryptor = SetupAccessTokenDecryptor();

            var exception = Record.ExceptionAsync(() => accessTokenDecryptor.InvokeAsync(httpContext));

            Assert.Null(await exception);
        }

        [Fact]
        public async Task AccessToken_Decryptor_With_Null_Token_Invokes_Next_Middleware()
        {
            var httpContext = SetupHttpContextClassNullToken();
            var accessTokenDecryptor = SetupAccessTokenDecryptor();

            var exception = Record.ExceptionAsync(() => accessTokenDecryptor.InvokeAsync(httpContext));

            Assert.Null(await exception);
        }

        [Fact]
        public async Task AccessToken_Decryptor_With_Empty_Token_Invokes_Next_Middleware()
        {
            var httpContext = SetupHttpContextClassEmptyToken();
            var accessTokenDecryptor = SetupAccessTokenDecryptor();

            var exception = Record.ExceptionAsync(() => accessTokenDecryptor.InvokeAsync(httpContext));

            Assert.Null(await exception);
        }
    }
}
