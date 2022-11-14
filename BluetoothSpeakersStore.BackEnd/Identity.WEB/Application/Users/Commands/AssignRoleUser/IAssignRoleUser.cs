namespace Identity.API.Application.Users.Commands.AssignRoleUser
{
    public interface IAssignRoleUser
    {
        Task<bool> AssignRoleToUserAsync(Guid id);
    }
}