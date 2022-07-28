using Domain.Users;

namespace Application.Users.Commands.UserUpdate
{
    public interface IUserUpdate
    {
        CreateUserModel Execute(UpdateUserModel user);
    }
}
