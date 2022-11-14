namespace Identity.API.Application.Users.Commands.LoginUser
{
    public interface ILoginUser
    {
        Task<string>LoginAsync(string username, string password);
    }
}