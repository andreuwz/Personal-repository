using Domain.Users;

namespace Application.Users.Queries.GetUser
{
    public interface IGetUser
    {
        User Execute(int id);
    }
}
