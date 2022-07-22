using Domain.Users;

namespace Application.Users.Commands.UserLogin
{
    public interface IUserLogin
    {
        User Execute(string username, string password);
    }
}
