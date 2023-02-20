using AutoMapper;
using Identity.API.Application.Users.Validations;
using Identity.API.DTO.Request;
using Identity.API.Persistence.RepositoryContract;

namespace Identity.API.Application.Users.Commands.UpdateUser
{
    public class UpdateLoggedUser : IUpdateLoggedUser
    {
        private readonly IIdentityRepository repository;
        private readonly IUserValidations userValidations;
        private readonly IMapper mapper;

        public UpdateLoggedUser(IIdentityRepository repository, IUserValidations userValidations, 
            IMapper mapper)
        {
            this.repository = repository;
            this.userValidations = userValidations;
            this.mapper = mapper;
        }

        public async Task<bool> UpdateUserAsync(string id, EditUserModel userModel)
        {
            var userForUpdate = await repository.FindByIdAsync(id);

            userValidations.EnsureUserExists(userForUpdate);
            await userValidations.EnsureEditedEmailSameAsUserModelAsync(userForUpdate, userModel.Email);
            await userValidations.EnsureEditedUsernameSameAsUserModelAsync(userForUpdate, userModel.UserName);

            mapper.Map(userModel, userForUpdate);
            userForUpdate.ModifiedAt = DateTime.Now.Date;

            await repository.UpdateUserDataAsync(userForUpdate);
            return true;
        }
    }
}
