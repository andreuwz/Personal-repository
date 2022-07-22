using Domain.Users;

namespace Application.Users.Commands.UserUpdate
{
    public interface IUserUpdate
    {
        User Execute(User user);
    }
}
