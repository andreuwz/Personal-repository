namespace API.Gateway.Services.Contracts
{
    public interface IDataSecurityService
    {
        Task<string> DecryptData(string value);
        Task<string> EncryptData(string value);
    }
}