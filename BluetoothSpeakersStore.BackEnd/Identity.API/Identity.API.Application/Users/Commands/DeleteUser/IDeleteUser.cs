using Identity.API.Domain;

namespace Identity.API.Application.Users.Commands.DeleteUser
{
    public interface IDeleteUser
    {
        Task<User> DeleteUserAsync(Guid id);
    }
}