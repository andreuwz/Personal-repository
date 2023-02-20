using AutoMapper;
using Identity.API.Application.Users.Validations;
using Identity.API.Domain;
using Identity.API.Persistence.RepositoryContract;

namespace Identity.API.Application.Users.Commands.DeleteUser
{
    public class DeleteUser : IDeleteUser
    {
        private readonly IIdentityRepository repository;
        private readonly IUserValidations userValidations;
        private readonly IMapper mapper;

        public DeleteUser(IIdentityRepository repository, IUserValidations userValidations, 
            IMapper mapper)
        {
            this.repository = repository;
            this.userValidations = userValidations;
            this.mapper = mapper;
        }

        public async Task<User> DeleteUserAsync(Guid id)
        {
            var userForDelete = await repository.FindByIdAsync(id.ToString());
            userValidations.EnsureUserExists(userForDelete);

            await repository.DeleteUserAsync(userForDelete);
            return userForDelete;
        }
    }
}
