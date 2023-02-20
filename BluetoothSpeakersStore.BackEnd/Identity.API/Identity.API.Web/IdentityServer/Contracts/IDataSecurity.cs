namespace Identity.API.Web.IdentityServer.Contracts
{
    public interface IDataSecurity
    {
        Task<string> DecryptData(string value);
        Task<string> EncryptData(string value);
    }
}