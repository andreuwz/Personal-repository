using API.Gateway.Services.Contracts;
using NETCore.Encrypt;

namespace API.Gateway.Services
{
    public class DataSecurityService : IDataSecurityService
    {
        private readonly IConfiguration configuration;
        public DataSecurityService(IConfiguration configuration)
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
