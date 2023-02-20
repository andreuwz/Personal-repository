using API.Gateway.Models;

namespace API.Gateway.Test
{
    public class TokenServiceTest : ServicesTestBase
    {
        [Fact]
        public async Task TrimAccessToken_Returns_Trimmed_Token_Which_Previously_Contained_Bearer_Word()
        {
            var inputString = "Bearer 555";
            var tokenService = SetupMockedTokenService();

            var trimmedToken = await tokenService.TrimAccessToken(inputString);
            var expectedToken = "555";

            Assert.Equal(expectedToken, trimmedToken);
        }

        [Fact]
        public async Task TrimAccessToken_Returns_Trimmed_Token_Which_Doesnt_Contain_Bearer_Word()
        {
            var inputString = "555";
            var tokenService = SetupMockedTokenService();

            var trimmedToken = await tokenService.TrimAccessToken(inputString);

            Assert.Equal(inputString, trimmedToken);
        }

        [Fact]
        public async Task Pass_Empty_String_Throws_Error_In_TrimAccessToken()
        {
            string inputString = null;
            var tokenService = SetupMockedTokenService();

            var exception = await Record.ExceptionAsync(async () => await tokenService.TrimAccessToken(inputString));
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task TrimAccessToken_Will_Not_Throw_Exception_When_Empty_String_Passed()
        {
            string inputString = string.Empty;
            var tokenService = SetupMockedTokenService();

            var exception = await Record.ExceptionAsync(async ()=> await tokenService.TrimAccessToken(inputString));
            Assert.Null(exception);
        }

        [Fact]
        public async Task Extract_Token_Expiration_Existing_Claim_Is_Successfull()
        {
            var tokenService = SetupMockedTokenService();

            var exception = await Record.ExceptionAsync(async () => await tokenService.ExtractTokenClaimValue(SetupHttpContextClassWithDecryptedToken(), "exp"));
            Assert.Null(exception);
        }

        [Fact]
        public async Task Extract_Token_Wrong_Claim_Is_Not_Successfull()
        {
            var tokenService = SetupMockedTokenService();

            Assert.Null(await tokenService.ExtractTokenClaimValue(SetupHttpContextClassWithDecryptedToken(), "wrongClaim"));
        }

        [Fact]
        public async Task Extract_Token_Expiration_Claim_When_Token_IsNull_Returns_Null()
        {
            var tokenService = SetupMockedTokenService();

            Assert.Null(await tokenService.ExtractTokenClaimValue(SetupHttpContextClassNullToken(), "exp"));
        }

        [Fact]
        public async Task Extract_Token_Wrong_Claim_When_Token_IsNull_Returns_Null()
        {
            var tokenService = SetupMockedTokenService();

            Assert.Null(await tokenService.ExtractTokenClaimValue(SetupHttpContextClassNullToken(), "wrongClaim"));
        }

        [Fact]
        public async Task Extract_Token_When_Claim_IsNull_Will_Give_Null()
        {
            string nullClaim = null;
            var tokenService = SetupMockedTokenService();

            Assert.Null(await tokenService.ExtractTokenClaimValue(SetupHttpContextClassNullToken(), nullClaim));
        }

        [Fact]
        public async Task Extract_Token_Empty_Claim_Will_Give_Null()
        {
            string emptyClaim = string.Empty;
            var tokenService = SetupMockedTokenService();

            Assert.Null(await tokenService.ExtractTokenClaimValue(SetupHttpContextClassNullToken(), emptyClaim));
        }

        [Fact]
        public async Task Decrypt_Proper_Encrypted_Token_Is_Successful()
        {
            var tokenService = SetupMockedTokenService();

           var exception = await Record.ExceptionAsync(async ()=> await tokenService.DecryptToken(SetupHttpContextClassWithEncryptedToken()));
            Assert.Null(exception);
        }


        [Fact]
        public async Task Decrypt_Null_Token_Returns_Empty_String()
        {
            var tokenService = SetupMockedTokenService();

            Assert.Equal("",await tokenService.DecryptToken(SetupHttpContextClassNullToken()));
        }

        [Fact]
        public async Task Decrypt_Empty_Token_Returns_Empty_String()
        {
            var tokenService = SetupMockedTokenService();

            Assert.Equal("", await tokenService.DecryptToken(SetupHttpContextClassEmptyToken()));
        }

        [Fact]
        public async Task Decrypt_Wrong_Format_Token_Throws_Exception()
        {
            var tokenService = SetupMockedTokenService();

            var exception = await Record.ExceptionAsync(async ()=> await tokenService.DecryptToken(SetupHttpContextClassWrongFormatToken()));
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task Issue_New_AccessToken_Via_Proper_RefreshToken_Is_Successful()
        {
            var tokenService = SetupMockedTokenService();
            var httpClientFactory = SetupMockedHttpClientFactoryWithResponseOkay();
            var httpContext = SetupHttpContextClassWithEncryptedRefreshToken();

            Assert.NotNull(await tokenService.IssueNewAccessTokenViaRefreshToken(httpContext, httpClientFactory));
        }

        [Fact]
        public async Task Issue_New_AccessToken_Via_Improper_RefreshToken_Is_Not_Successful()
        {
            var tokenService = SetupMockedTokenService();
            var httpClientFactory = SetupMockedHttpClientFactoryWithResponse500();
            var httpContext = SetupHttpContextClassWithEncryptedRefreshToken();

            var exception = await Record.ExceptionAsync(async ()=> await tokenService.IssueNewAccessTokenViaRefreshToken(httpContext, httpClientFactory));
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task Apply_New_Tokens_To_Http_Headers_Throws_Exception_When_Headers_Are_Missing()
        {
            var tokenService = SetupMockedTokenService();
            var httpContext = SetupHttpContextClassWithMissingHeaders();
            var tokenModel = new TokenModel()
            {
                AccessToken = "test",
                RefreshToken = "test"
            };

            var exception = Record.Exception(() => tokenService.ApplyNewTokensToHttpHeaders(httpContext, tokenModel));
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task Apply_New_Tokens_To_Http_Headers_Is_Successful()
        {
            var tokenService = SetupMockedTokenService();
            var httpContext = SetupHttpContextWithProperAccessAndRefreshTokens();
            var tokenModel = new TokenModel()
            {
                AccessToken = "test",
                RefreshToken = "test"
            };

            var exception = Record.Exception(() =>
            {
                tokenService.ApplyNewTokensToHttpHeaders(httpContext, tokenModel);
            });

            Assert.Null(exception);
        }

        [Fact]
        public async Task Apply_New_Null_Tokens_Throws_Error()
        {
            var tokenService = SetupMockedTokenService();
            var httpContext = SetupHttpContextWithProperAccessAndRefreshTokens();
            TokenModel tokenModel = null;

            var exception = Record.Exception(() =>
            {
                tokenService.ApplyNewTokensToHttpHeaders(httpContext, tokenModel);
            });

            Assert.NotNull(exception);
        }
    }
}
