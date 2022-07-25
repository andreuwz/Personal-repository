using Domain.Users;

namespace Application.Users.Commands.UserUpdate
{
    public interface IUserUpdate
    {
        UserModel Execute(User user);
    }
}
