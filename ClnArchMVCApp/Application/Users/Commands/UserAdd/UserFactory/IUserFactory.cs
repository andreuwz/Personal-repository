using Domain.Users;

namespace Application.Users.Commands.UserAdd.UserFactory
{
    public interface IUserFactory
    {
        User Execute(string username, string password, string firstname, bool isAdmin, DateTime createdAt);
    }
}
