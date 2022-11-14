using NETCore.Encrypt;

namespace API.Gateway.Common
{
    public class DataSecurity
    {
        private readonly IConfiguration configuration;
        public DataSecurity(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> EncryptData(string value)
        {
            var securityKey = configuration["SecurityKey"];
            return EncryptProvider.AESEncrypt(value, securityKey);
        }

        public async Task<string> DecryptData(string value)
        {
            var securityKey = configuration["SecurityKey"];
            return EncryptProvider.AESDecrypt(value, securityKey);
        }
    }
}
