namespace API.Gateway.Test
{
    public class DataSecurityServiceTest : ServicesTestBase
    {

        [Fact]
        public async Task EncryptData_Method_Successfully_Returns_Correct_Encrypted_String()
        {
            var stringForEncryption = "test";
            var dataSecurityService = SetupMockedDataSecurityServiceWithProperKey();

            var encryptedString = await dataSecurityService.EncryptData(stringForEncryption);
            var expectedString = "GNf+87pZTzOwIfHiydbSKA==";

            Assert.Equal(expectedString, encryptedString);
        }

        [Fact]
        public async Task DecryptData_Method_Successfully_Returns_Correct_Decrypted_String()
        {
            var stringForDecryption = "GNf+87pZTzOwIfHiydbSKA==";
            var dataSecurityService = SetupMockedDataSecurityServiceWithProperKey();

            var resultantString = await dataSecurityService.DecryptData(stringForDecryption);
            var expectedString = "test";

            Assert.Equal(expectedString, resultantString);
        }

        [Fact]
        public async Task EncryptData_Method_Without_Key_Not_Possible()
        {
            var stringForEncryption = "test";
            var dataSecurityService = SetupMockedDataSecurityServiceWithoutProperKey();

            var exception = await Record.ExceptionAsync(async () => await dataSecurityService.EncryptData(stringForEncryption));  
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task DecryptData_Method_Without_Key_Not_Possible()
        {
            var stringForDecryption = "GNf+87pZTzOwIfHiydbSKA==";
            var dataSecurityService = SetupMockedDataSecurityServiceWithoutProperKey();

            var exception = await Record.ExceptionAsync(async () => await dataSecurityService.DecryptData(stringForDecryption));
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task Pass_Empty_String_To_EncryptMethod_Throws_Exception()
        {
            var dataSecurityService = SetupMockedDataSecurityServiceWithProperKey();

            var exception = await Record.ExceptionAsync(async ()=> await dataSecurityService.EncryptData(""));
            Assert.NotNull(exception);
        }
    }
}