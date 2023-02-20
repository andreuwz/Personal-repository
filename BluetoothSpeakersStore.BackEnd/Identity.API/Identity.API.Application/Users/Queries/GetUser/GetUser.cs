using Identity.API.Application.Users.Validations;
using Identity.API.Domain;
using Identity.API.Persistence.RepositoryContract;

namespace Identity.API.Application.Users.Queries.GetUser
{
    public class GetUser : IGetUser
    {
        private readonly IIdentityRepository repository;
        private readonly IUserValidations userValidation;

        public GetUser(IIdentityRepository repository, IUserValidations userValidation)
        {
            this.repository = repository;
            this.userValidation = userValidation;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var foundUser = await repository.FindByIdAsync(id);
            
            userValidation.EnsureUserExists(foundUser);

            return foundUser;
        }
    }
}
