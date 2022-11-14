namespace Identity.API.Application.Users.Commands.UnassignRoleUser
{
    public interface IUnassignRoleUser
    {
        Task<bool> UnassignUserFromRoleAsync(Guid id);
    }
}