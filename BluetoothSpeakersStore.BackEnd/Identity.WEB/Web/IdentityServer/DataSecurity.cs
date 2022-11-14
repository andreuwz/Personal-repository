using Identity.API.Web.IdentityServer.Contracts;
using NETCore.Encrypt;

namespace Identity.API.Web.IdentityServer
{
    internal class DataSecurity : IDataSecurity
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
