using Identity.API.Application.Users.Validations;
using Identity.API.Persistence.RepositoryContract;

namespace Identity.API.Application.Users.Commands.UnassignRoleUser
{
    internal class UnassignRoleUser : IUnassignRoleUser
    {
        private readonly IUserValidations userValidations;
        private readonly IIdentityRepository repository;

        public UnassignRoleUser(IUserValidations userValidations, IIdentityRepository repository)
        {
            this.userValidations = userValidations;
            this.repository = repository;
        }

        public async Task<bool> UnassignUserFromRoleAsync(Guid id)
        {
            var user = await repository.FindByIdAsync(id.ToString());
            userValidations.EnsureUserExists(user);
            await userValidations.EnsureUserInRoleAsync(id.ToString(), "Administrator");

            await repository.ManualRemoveRoleFromUserAsync(user, "Administrator");
            return true;
        }
    }
}
