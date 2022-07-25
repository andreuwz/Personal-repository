using Domain.Users;

namespace Application.Users.Queries.GetAllUsers
{
    public interface IGetAllUsers
    {
        IEnumerable<UserModel> Execute();
    }
}
