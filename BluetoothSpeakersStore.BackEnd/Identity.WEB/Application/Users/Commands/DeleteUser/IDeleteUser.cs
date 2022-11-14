namespace Identity.API.Application.Users.Commands.DeleteUser
{
    public interface IDeleteUser
    {
        Task<bool> DeleteUserAsync(Guid id);
    }
}