using Domain.Users;

namespace Application.Users.Commands.UserLogin
{
    public interface IUserLogin
    {
        UserModel Execute(string username, string password);
    }
}
