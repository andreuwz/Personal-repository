using AutoMapper;
using Identity.API.Application.Users.Validations;
using Identity.API.Common.Constants;
using Identity.API.Domain;
using Identity.API.DTO.Request;
using Identity.API.DTO.Response;
using Identity.API.Persistence.RepositoryContract;
using Identity.API.Web.AsyncMessageBusServices.PublishMessages;
using System.Text.Json;

namespace Identity.API.Application.Users.Commands.UpdateUserAdmin
{
    internal class UpdateUserAdmin : IUpdateUserAdmin
    {
        private readonly IIdentityRepository repository;
        private readonly IUserValidations userValidations;
        private readonly IMapper mapper;
        private readonly IPublishNewMessage publishMessage;

        public UpdateUserAdmin(IIdentityRepository repository, IUserValidations userValidations,
            IMapper mapper, IPublishNewMessage publisMessage)
        {
            this.repository = repository;
            this.userValidations = userValidations;
            this.mapper = mapper;
            this.publishMessage = publisMessage;
        }

        public async Task<bool> UpdateUserAsync(Guid id, AdminEditUserModel userModel)
        {
            var userForUpdate = await repository.FindByIdAsync(id.ToString());
            
            userValidations.EnsureUserExists(userForUpdate);
            await userValidations.EnsureEditedEmailSameAsUserModelAsync(userForUpdate, userModel.Email);
            await userValidations.EnsureEditedUsernameSameAsUserModelAsync(userForUpdate, userModel.UserName);

            mapper.Map(userModel, userForUpdate);
            userForUpdate.ModifiedAt = DateTime.Now.Date;

            await repository.UpdateUserDataAsync(userForUpdate);
            await PublishUpdatedUserDataMessageAsync(userForUpdate);
                
            return true;
        }

        private async Task PublishUpdatedUserDataMessageAsync(User userForUpdate)
        {
            var publishUpdatedUser = mapper.Map<PublishUpdatedUserModel>(userForUpdate);
            publishUpdatedUser.EventType = AppConstants.eventTypeUpdatedUserInfo;
            publishUpdatedUser.Roles = await repository.GetUserRolesAsync(userForUpdate);

            var message = JsonSerializer.Serialize(publishUpdatedUser);
            publishMessage.PublishMessage(message);
        }
    }
}
