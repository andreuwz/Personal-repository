using API.Gateway.Middleware;
using API.Gateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;

namespace API.Gateway.Test
{
    public class ServicesTestBase
    {
        protected string encryptedRefreshToken = "ZBwa2GBvc71Ec5IW7/c9DJmxTKTr2kW3QJXIlTn6IZjJI3pemmj4+IPNIfbmYanB6OpwHyIgl/qZBJKgvL1WkqmPngUu7yFzjEkkQhjM0Pg=";
        protected string decryptedRefreshToken = "FEED30A05FCBB44C8C4B80C7C8998D6D5015F34A9AF8B71B43C79D1569F70FA8-1";
        protected string encryptedAccessToken = "wxV84ZRiXr+/gsAOZ/MYSDB3IUS8DhKMngZh9OvcANsqV8Qsgj/M5L2AOh4I2fe8qp1MstYElf4PimzEONboJiIGZ45smCVRsjpWzIltWT01Gkf1MTt6MN3DaqD34qXfIMDyMvoGPeTqh5jmytyLeL2TWzfu+/EkFYlpJtdSJs8reOPxRNmHAU0e/yTMl3rZ14zYcYAnb26KFmFlbLCCjwgV/SmoNpTKIn3mL+k3VOmXaRasJG2sa4uzDCwQKswenLIKhyXayzIcLXc3Su7Zecd9X6m22VwDmoACXYKoNqCFEWJ53lmEhJTnlsqjfZLs2TEgACRnGbR2FxM/qyooQV4uoiih+OgPwfww+bGr9CS0FMvI3JvKHltQy/EM03FNTB6v0KpVuPtTJBnohOlEMLZYP1thoIoc9bBdOSs8SadCZ/W3DojFe4I+2rI1KclmA/K1UETeQcktwJ+5WitoFiceQw81hWuUvMbebZnyuqdxKno9YN/Yn7LOHnblGdnVpp1IrQefulblJs7BpZFhi1dnum36mVrugv3NxSzSpkljJLcDf1Zw4NkrGHR8O0ThLR1B9CPe2lBMMu2nj/pikt8g9QEee9r18vuyO/jgusSPaTlnp+Jag1ZoLYasbIfxcgla/84UAURMCWRzQKwFOePoH/aL5XD852FdipqsE4aeh4GwoFbBFvtpOdIKx7vP5lXeSRW0njQyPrZrPSbt5WbICYU60LD85I7wa1wng29wzmqcvZkOWcUk/Um8jZyir/e49YbAqv/ZlHzIJFyp+HJGbM/0mVhR7SxA+ay1KUuMGJ8mUjrugNvNfcZ+SXIAp6OqrhrNkSskzskK1h94Gu0RPtZM6tkA1PmTSlgUrZwcQ4cJNKqxqcTwKIbw41+3CacptSEUCVeMjCCXi48+XnVtpJUJbDdbhgmiIw1kiPhW0JFBZY+Lzl2v5VBmp5R7FTqzzAmAxho2ofyIKLawnPUCzNg1Zn/jtCPpVl+AmThguFbBqgxVvFOBpxR1KhzIkVPnXOE8OYT83K2a5t7wD4Z6byBF2FdDhSeGMmlrpm7kkaUEB8hDQvic1L1pKFKFNIb2no9RDRLsSxPkCsdI7+w8hp/xTI9l0Uqlglegc1rVgoju6PAu7tFTbr5mjRILyLYQ4Nc02OKLQtHR4kOJ+iAwG0VOZEyWc8SdY4wfsx20RlZGJFFkDfMQAC5grb3tzrteUQ+PgK0Cxwybdg2ZAbkoAak/gnIa3MBor1hpdPfXBP13pN6QhW8dMQvyltfH7JQ15RvNb+JA8AQQn94cbViRBqnjflDGfpzB2b6ljkXS5gY+4JgH5iHkPFpKP8jLsELrXa2NUpxEG7gPY/UHuYgrZjA4C6yrl2c6kwlhkxnqXNhxZqT1LkHGPrKRZ6+7mYl/Y/u51ZX5lxRXZuDhkCcc5fMCQwRfDwLBFdnthFSvgqbUE65enshZ35Whx6VpYAyHsFcJWtLQQZbZOBu03P3Cgln2HrXRt0BjflAUPTL8/GRLWcKO7rsQkJ+ohnkl3h5JkchlCH+Rk/ZLab7+FCbXPDeGImPDvy+cbOHPgEEg92iKjPir/tRq9p4a3hKk/P/mAzPq5R9VQ80wweWKJ1YYAfjpfuqN4NBJDWTM6IYrPfolHGKBCr4IuNk+OsPVckj7Rl2lqOW9HKG0GXlLW5OFcf6e9PUmHx5+UEBQS+I=";
        protected string decryptedAccessToken = "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjNCMjU5MTZGNDJFNEE4M0U0QTU5QUM3RUZFNTJCQUQ3IiwidHlwIjoiYXQrand0In0.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MjU0IiwibmJmIjoxNjc0ODA3NDk1LCJpYXQiOjE2NzQ4MDc0OTUsImV4cCI6MTY3NDgwNzc5NSwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI1NC9yZXNvdXJjZXMiLCJzY29wZSI6WyJDYXJ0LkFQSSIsIkNhdGFsb2d1ZS5BUEkiLCJJZGVudGl0eS5BUEkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsicGFzc3dvcmQiXSwiY2xpZW50X2lkIjoiQW5ndWxhckNsaWVudCIsInN1YiI6Ijg4ZThmOWU1LWE5NDgtNDhkYy1hNjg4LTAxZGFkZTFhYWY4YiIsImF1dGhfdGltZSI6MTY3NDgwNzQ5NSwiaWRwIjoibG9jYWwiLCJJZCI6Ijg4ZThmOWU1LWE5NDgtNDhkYy1hNjg4LTAxZGFkZTFhYWY4YiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJBbmRyZXlAYWJ2LmJnIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjpbIkFkbWluaXN0cmF0b3IiLCJNYXN0ZXJBZG1pbiJdLCJNYXN0ZXJBZG1pbiI6InRydWUiLCJBZG1pbmlzdHJhdG9yIjoidHJ1ZSIsImp0aSI6IjI3OUFGNEExNjFGODc2MTkyN0E2QjNGNUQ3ODA4OEJGIn0.qlAm_dFzYdbEQTDdW4QqibuRY-SXmeN_2o8qfezqAe_nhKGQWN-VW8TyYoH4IGllSNQayAvym_-_OpmsP2HEjeXtURiq3QBZugxvWVZtthuxh1qmuq5HYVhL7H5Mj-r4vc86nt6zGqsg8pQrcNOVRKCqhpIgFGZ7T0NmmizZ8WNCqmaxlVJIfFvXIy9rlU-q6a5xz72Xkf0h4VfFoyqadV6KpcLjaRiV7DbKBD7xX4e5FEMXb4xS5u9TCdkghz0ctCjGgeTuiv5FkrDKTz7kj_NAZGmfHea7heQN8psI5_pZ35u7sQb0RDiuccIFKDyRtjrhvLUNfXdmMUg_F02sJw";
        protected string outputTokenPair = "{\"access_token\":\"eyJhbGciOiJSUzI1NiIsImtpZCI6IjNCMjU5MTZGNDJFNEE4M0U0QTU5QUM3RUZFNTJCQUQ3IiwidHlwIjoiYXQrand0In0.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MjU0IiwibmJmIjoxNjc0ODA4ODIyLCJpYXQiOjE2NzQ4MDg4MjIsImV4cCI6MTY3NDgwOTEyMiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI1NC9yZXNvdXJjZXMiLCJzY29wZSI6WyJDYXJ0LkFQSSIsIkNhdGFsb2d1ZS5BUEkiLCJJZGVudGl0eS5BUEkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsicGFzc3dvcmQiXSwiY2xpZW50X2lkIjoiQW5ndWxhckNsaWVudCIsInN1YiI6Ijg4ZThmOWU1LWE5NDgtNDhkYy1hNjg4LTAxZGFkZTFhYWY4YiIsImF1dGhfdGltZSI6MTY3NDgwODgyMiwiaWRwIjoibG9jYWwiLCJJZCI6Ijg4ZThmOWU1LWE5NDgtNDhkYy1hNjg4LTAxZGFkZTFhYWY4YiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJBbmRyZXlAYWJ2LmJnIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjpbIkFkbWluaXN0cmF0b3IiLCJNYXN0ZXJBZG1pbiJdLCJNYXN0ZXJBZG1pbiI6InRydWUiLCJBZG1pbmlzdHJhdG9yIjoidHJ1ZSIsImp0aSI6IkY0QkFGNTE0NDgwNDg5MTZBOUVGNTlDMzNEQzQyOEFCIn0.VJQU2lVF-OuItGSatDRXvRl2TahiK5yuMR0ZYyEqnZeGUFpUM7UJdwL-RBAi10QLFTcsnGarIu-f_w5X_ScPqh-VQaE9vVZ_MR7giiNipCH_xjZw6J3sKBHvdoPWpEU7oaQw7W5STfFJlTyQechw9WQ4vSicRgcJyDkh7-DSz9Xco3zo0ccdXSNiKndyjCHlEfVY4EEvUFs5JYQ7QlFwTo0rql3Gp401A-yOi_DAusHtfY_7fIu3tjVYLjwpkltwHCSFogW69PGJ6jjVM_cX7XkOHHj5V45cZ7gFvfshpxVfmbz5TjH1ne4J8Yxdc_qt6TEFeOfbBqzfe7v5Y_WBYg\",\"expires_in\":300,\"token_type\":\"Bearer\",\"refresh_token\":\"D95DCD195E0857885F46F9CBF5F5C4848C25B975CAD349D01072AC982449AF8D-1\",\"scope\":\"Cart.API Catalogue.API Identity.API offline_access\"}";
        
        protected HttpContext SetupCompleteHttpContext()
        {
            var mockedContext = new Mock<HttpContext>();

            mockedContext.Setup(c => c.Request.Headers["Authorization"])
                .Returns(decryptedAccessToken);
            mockedContext.Setup(c => c.Request.Headers["RefreshAuth"])
                .Returns(encryptedRefreshToken);

            return mockedContext.Object;
        }

        protected IHttpClientFactory SetupMockedHttpClientFactoryWithResponseOkay()
        {
            var messageHandlerMock = new Mock<HttpMessageHandler>();

            messageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(async (HttpRequestMessage request, CancellationToken token) => {

                    string requestMessageContent = await request.Content.ReadAsStringAsync();

                    HttpResponseMessage response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(outputTokenPair)
                    };

                    return response;
                })
                .Verifiable();

            var mockedHttpClient = new HttpClient(messageHandlerMock.Object);

            var mockedClientFactory = new Mock<IHttpClientFactory>();
            mockedClientFactory
                .Setup(f => f.CreateClient(It.IsAny<string>()))
                    .Returns(mockedHttpClient);

            return mockedClientFactory.Object;
        }

        protected IHttpClientFactory SetupMockedHttpClientFactoryWithResponseOkayAndNewAccessToken()
        {
            var messageHandlerMock = new Mock<HttpMessageHandler>();

            messageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(async (HttpRequestMessage request, CancellationToken token) => {

                    string requestMessageContent = await request.Content.ReadAsStringAsync();

                    HttpResponseMessage response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(outputTokenPair)
                    };

                    return response;
                })
                .Verifiable();

            var mockedHttpClient = new HttpClient(messageHandlerMock.Object);

            var mockedClientFactory = new Mock<IHttpClientFactory>();
            mockedClientFactory
                .Setup(f => f.CreateClient(It.IsAny<string>()))
                    .Returns(mockedHttpClient);

            return mockedClientFactory.Object;
        }

        protected IHttpClientFactory SetupMockedHttpClientFactoryWithResponse500()
        {
            var messageHandlerMock = new Mock<HttpMessageHandler>();

            messageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(async (HttpRequestMessage request, CancellationToken token) => {

                    string requestMessageContent = await request.Content.ReadAsStringAsync();

                    HttpResponseMessage response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Content = new StringContent("")
                    };

                    return response;
                })
                .Verifiable();

            var mockedHttpClient = new HttpClient(messageHandlerMock.Object);

            var mockedClientFactory = new Mock<IHttpClientFactory>();
            mockedClientFactory
                .Setup(f => f.CreateClient(It.IsAny<string>()))
                    .Returns(mockedHttpClient);

            return mockedClientFactory.Object;
        }

        protected DataSecurityService SetupMockedDataSecurityServiceWithProperKey()
        {
            var mockedConfiguration = new Mock<IConfiguration>();

            mockedConfiguration.SetupGet(config => config["SecurityKey"])
                .Returns("mCg2O2XksLbqAtIZR4eJzO77zA2RSce4");

            return new DataSecurityService(mockedConfiguration.Object);
        }

        protected DataSecurityService SetupMockedDataSecurityServiceWithoutProperKey()
        {
            var mockedConfiguration = new Mock<IConfiguration>();

            return new DataSecurityService(mockedConfiguration.Object);
        }

        protected TokenService SetupMockedTokenService()
        {
            return new TokenService(SetupMockedDataSecurityServiceWithProperKey());
        }

        protected HttpContext SetupHttpContextClassWithDecryptedToken()
        {
            var mockedHttpContext = new Mock<HttpContext>();

            mockedHttpContext.Setup(c => c.Request.Headers["Authorization"])
                .Returns("Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjNCMjU5MTZGNDJFNEE4M0U0QTU5QUM3RUZFNTJCQUQ3IiwidHlwIjoiYXQrand0In0.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MjU0IiwibmJmIjoxNjc0NTY0MzU0LCJpYXQiOjE2NzQ1NjQzNTQsImV4cCI6MTY3NDU2NDY1NCwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI1NC9yZXNvdXJjZXMiLCJzY29wZSI6WyJDYXJ0LkFQSSIsIkNhdGFsb2d1ZS5BUEkiLCJJZGVudGl0eS5BUEkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsicGFzc3dvcmQiXSwiY2xpZW50X2lkIjoiQW5ndWxhckNsaWVudCIsInN1YiI6IjEyMGQzMmZlLTU2ZjAtNDljYS1iNTc2LWQzMjFmMDNmZDYyYiIsImF1dGhfdGltZSI6MTY3NDU2NDM1NCwiaWRwIjoibG9jYWwiLCJJZCI6IjEyMGQzMmZlLTU2ZjAtNDljYS1iNTc2LWQzMjFmMDNmZDYyYiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJGb25jaG8xMjM0IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVndWxhclVzZXIiLCJSZWd1bGFyVXNlciI6InRydWUiLCJqdGkiOiJDRDZBQjM1NTlDNjEzQjY3MkU2RjAwRDEyMzYzQkE0NCJ9.ZmISLsUZeSkXJI_LRu7zH9nVYzZXjDXB6VoXT7I9BIpM7lt52rM6U9gX44luv5I04M-5tg04oFNRymUbELwz90EaROMCc5BKM6M6xGnY55w5lY7_HkVdtyc1rDl-wQj8ypPs3JbOdDucz1e_zaoVreKnL6pXsaxTaDUKqmpqyGXYQl45uAUxSoPdn3DTBmLwR6PH_lL97x3ygBAuOGrap5rc1sH8rOXSP9JdRVCECLhtAvywdHzBKCa42o9b3U6ja1aM0lfVbYcsVv1ANr35nIz7pF06Ekv3WgCwsV04oShFajzx03ddxlHQnIpJ0c0zgMJnuCiPXV_65BLwMpJnfg");

            return mockedHttpContext.Object;
        }

        protected HttpContext SetupHttpContextClassWithEncryptedRefreshToken()
        {
            var mockedHttpContext = new Mock<HttpContext>();

            mockedHttpContext.Setup(c => c.Request.Headers["RefreshAuth"])
                .Returns(encryptedRefreshToken);

            return mockedHttpContext.Object;
        }

        protected HttpContext SetupHttpContextClassNullToken()
        {
            string testString = null;
            var mockedHttpContext = new Mock<HttpContext>();

            mockedHttpContext.Setup(c => c.Request.Headers["Authorization"])
                .Returns(testString);
            return mockedHttpContext.Object;
        }

        protected HttpContext SetupHttpContextClassWithEncryptedToken()
        {
            var mockedHttpContext = new Mock<HttpContext>();

            mockedHttpContext.Setup(c => c.Request.Headers["Authorization"])
                .Returns("wxV84ZRiXr+/gsAOZ/MYSDB3IUS8DhKMngZh9OvcANsqV8Qsgj/M5L2AOh4I2fe8qp1MstYElf4PimzEONboJiIGZ45smCVRsjpWzIltWT01Gkf1MTt6MN3DaqD34qXfIMDyMvoGPeTqh5jmytyLeL2TWzfu+/EkFYlpJtdSJs8reOPxRNmHAU0e/yTMl3rZ14zYcYAnb26KFmFlbLCCjwgV/SmoNpTKIn3mL+k3VOmXaRasJG2sa4uzDCwQKswenLIKhyXayzIcLXc3Su7Zecd9X6m22VwDmoACXYKoNqCFEWJ53lmEhJTnlsqjfZLs2TEgACRnGbR2FxM/qyooQV4uoiih+OgPwfww+bGr9CS0FMvI3JvKHltQy/EM03FNTB6v0KpVuPtTJBnohOlEMLZYP1thoIoc9bBdOSs8SadCZ/W3DojFe4I+2rI1KclmA/K1UETeQcktwJ+5WitoFiceQw81hWuUvMbebZnyuqdxKno9YN/Yn7LOHnblGdnVpp1IrQefulblJs7BpZFhi1dnum36mVrugv3NxSzSpkljJLcDf1Zw4NkrGHR8O0ThLR1B9CPe2lBMMu2nj/pikt8g9QEee9r18vuyO/jgusSPaTlnp+Jag1ZoLYasbIfxcgla/84UAURMCWRzQKwFOePoH/aL5XD852FdipqsE4aeh4GwoFbBFvtpOdIKx7vP5lXeSRW0njQyPrZrPSbt5WbICYU60LD85I7wa1wng29wzmqcvZkOWcUk/Um8jZyir/e49YbAqv/ZlHzIJFyp+HJGbM/0mVhR7SxA+ay1KUuMGJ8mUjrugNvNfcZ+SXIAp6OqrhrNkSskzskK1h94Gu0RPtZM6tkA1PmTSlgUrZwcQ4cJNKqxqcTwKIbw41+3CacptSEUCVeMjCCXi48+XnVtpJUJbDdbhgmiIw1kiPhW0JFBZY+Lzl2v5VBmp5R7FTqzzAmAxho2ofyIKLawnPUCzNg1Zn/jtCPpVl+AmThguFbBqgxVvFOBpxR1KhzIkVPnXOE8OYT83K2a5t7wD4Z6byBF2FdDhSeGMmlrpm7kkaUEB8hDQvic1L1pKFKFNIb2no9RDRLsSxPkCsdI7+w8hp/xTI9l0Uqlglegc1rVgoju6PAu7tFTbr5mjRILyLYQ4Nc02OKLQtHR4kOJ+iAwG0VOZEyWc8SdY4wfsx20RlZGJFFkDfMQAC5grb3tzrteUQ+PgK0Cxwybdg2ZAbkoAak/gnIa3MBor1hpdPfXBP13pN6QhW8dMQvyltfH7JQ15RvNb+JA8AQQn94cbViRBqnjflDGfpzB2b6ljkXS5gY+4JgH5iHkPFpKP8jLsELrXa2NUpxEG7gPY/UHuYgrZjA4C6yrl2c6kwlhkxnqXNhxZqT1LkHGPrKRZ6+7mYl/Y/u51ZX5lxRXZuDhkCcc5fMCQwRfDwLBFdnthFSvgqbUE65enshZ35Whx6VpYAyHsFcJWtLQQZbZOBu03P3Cgln2HrXRt0BjflAUPTL8/GRLWcKO7rsQkJ+ohnkl3h5JkchlCH+Rk/ZLab7+FCbXPDeGImPDvy+cbOHPgEEg92iKjPir/tRq9p4a3hKk/P/mAzPq5R9VQ80wweWKJ1YYAfjpfuqN4NBJDWTM6IYrPfolHGKBCr4IuNk+OsPVckj7Rl2lqOW9HKG0GXlLW5OFcf6e9PUmHx5+UEBQS+I=");

            return mockedHttpContext.Object;
        }

        protected HttpContext SetupHttpContextClassEmptyToken()
        {
            string testString = string.Empty;
            var mockedHttpContext = new Mock<HttpContext>();

            mockedHttpContext.Setup(c => c.Request.Headers["Authorization"])
                .Returns(testString);
            return mockedHttpContext.Object;
        }

        protected HttpContext SetupHttpContextClassWrongFormatToken()
        {
            string testString = "Bearer myToken";
            var mockedHttpContext = new Mock<HttpContext>();

            mockedHttpContext.Setup(c => c.Request.Headers["Authorization"])
                .Returns(testString);
            return mockedHttpContext.Object;
        }

        protected HttpContext SetupHttpContextClassWithMissingHeaders()
        {
            return new Mock<HttpContext>().Object;
        }

        protected HttpContext SetupHttpContextWithProperAccessAndRefreshTokens()
        {
            var mockedHttpContext = new Mock<HttpContext>();

            mockedHttpContext.Setup(c => c.Request.Headers["Authorization"])
                .Returns("Access_token");
            mockedHttpContext.Setup(c => c.Request.Headers["RefreshAuth"])
               .Returns("Refresh_token");

            return mockedHttpContext.Object;
        }

        protected RequestDelegate SetupSuccessfulRequestDelegate()
        {
            var mockedRequestDelegate = new Mock<RequestDelegate>();
            mockedRequestDelegate.Setup(x => x.Invoke(It.IsAny<HttpContext>()))
                .Callback<HttpContext>(async context => await context.Response.WriteAsync("true"));

            return mockedRequestDelegate.Object;
        }

        protected AccessTokenDecryptor SetupAccessTokenDecryptor()
        {
            var mockedSuccessfulRequestDelegate = SetupSuccessfulRequestDelegate();
            var mockedTokenService = SetupMockedTokenService();

            return new AccessTokenDecryptor(mockedSuccessfulRequestDelegate, mockedTokenService);
        }

        protected AccessTokenRefresher SetupAccessTokenRefresher()
        {
            var mockedRequestDeleagate = SetupSuccessfulRequestDelegate();
            var mockedTokenService = SetupMockedTokenService();
            var mockedHttpClientFactory = SetupMockedHttpClientFactoryWithResponseOkay();

            return new AccessTokenRefresher(mockedRequestDeleagate, mockedTokenService, mockedHttpClientFactory);
        }

        protected AccessTokenRefresher SetupAccessTokenRefresherIssuingNewAccessToken()
        {
            var mockedRequestDeleagate = SetupSuccessfulRequestDelegate();
            var mockedTokenService = SetupMockedTokenService();
            var mockedHttpClientFactory = SetupMockedHttpClientFactoryWithResponseOkayAndNewAccessToken();

            return new AccessTokenRefresher(mockedRequestDeleagate, mockedTokenService, mockedHttpClientFactory);
        }
    }
}
