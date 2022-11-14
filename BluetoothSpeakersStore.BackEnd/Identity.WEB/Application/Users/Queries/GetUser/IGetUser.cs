using Identity.API.Domain;

namespace Identity.API.Application.Users.Queries.GetUser
{
    public interface IGetUser
    {
        Task<User> GetUserByIdAsync(string id);
    }
}