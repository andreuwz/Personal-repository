using Identity.API.Application.Users.Validations;
using Identity.API.Persistence.RepositoryContract;

namespace Identity.API.Application.Users.Commands.AssignRoleUser
{
    public class AssignRoleUser : IAssignRoleUser
    {

        private readonly IUserValidations userValidations;
        private readonly IIdentityRepository repository;

        public AssignRoleUser(IUserValidations userValidations, IIdentityRepository repository)
        {
            this.userValidations = userValidations;
            this.repository = repository;
        }

        public async Task<bool> AssignRoleToUserAsync(Guid id)
        {
            var user = await repository.FindByIdAsync(id.ToString());
            userValidations.EnsureUserExists(user);
            await userValidations.EnsureUserNotInRoleAsync(id.ToString(), "Administrator");

            await repository.ManualAddRoleToUserAsync(user, "Administrator");

            return true;
        }
    }
}
