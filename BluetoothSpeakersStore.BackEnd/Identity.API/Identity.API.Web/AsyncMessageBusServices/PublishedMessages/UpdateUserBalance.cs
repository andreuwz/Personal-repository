using Identity.API.Application.Users.Validations;
using Identity.API.DTO.Request;
using Identity.API.Persistence.RepositoryContract;

namespace Identity.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public class UpdateUserBalance : IUpdateUserBalance
    {
        private readonly IIdentityRepository identityRepository;
        private readonly IUserValidations userValidations;

        public UpdateUserBalance(IIdentityRepository identityRepository, IUserValidations userValidations)
        {
            this.identityRepository = identityRepository;
            this.userValidations = userValidations;
        }

        public async Task<bool> UpdateUserBalanceAsync(PublishedUserBalanceModel userModel)
        {
            var userForUpdate = await identityRepository.FindByIdAsync(userModel.Id.ToString());
            userValidations.EnsureUserExists(userForUpdate);

            userForUpdate.Balance = userModel.Balance;
            await identityRepository.UpdateUserDataAsync(userForUpdate);
            return true;
        }
    }
}
