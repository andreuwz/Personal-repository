using AutoMapper;
using Identity.API.Application.Users.Validations;
using Identity.API.Common.Constants;
using Identity.API.Domain;
using Identity.API.DTO.Request;
using Identity.API.DTO.Response;
using Identity.API.Persistence.RepositoryContract;
using Identity.API.Web.AsyncMessageBusServices.PublishMessages;
using System.Text.Json;

namespace Identity.API.Application.Users.Commands.UpdateUser
{
    internal class UpdateLoggedUser : IUpdateLoggedUser
    {
        private readonly IIdentityRepository repository;
        private readonly IUserValidations userValidations;
        private readonly IMapper mapper;
        private readonly IPublishNewMessage publishNewMessage;

        public UpdateLoggedUser(IIdentityRepository repository, IUserValidations userValidations, 
            IMapper mapper, IPublishNewMessage publishNewMessage)
        {
            this.repository = repository;
            this.userValidations = userValidations;
            this.mapper = mapper;
            this.publishNewMessage = publishNewMessage;
        }

        public async Task<bool> UpdateUserAsync(string id, EditUserModel userModel)
        {
            var userForUpdate = await repository.FindByIdAsync(id);

            userValidations.EnsureUserExists(userForUpdate);
            await userValidations.EnsureEditedEmailSameAsUserModelAsync(userForUpdate, userModel.Email);
            await userValidations.EnsureEditedUsernameSameAsUserModelAsync(userForUpdate, userModel.UserName);

            mapper.Map(userModel, userForUpdate);
            userForUpdate.ModifiedAt = DateTime.Now.Date;

            await PublishUpdatedUserDataMessageAsync(userForUpdate);
            await repository.UpdateUserDataAsync(userForUpdate);
        
            return true;
        }

        private async Task PublishUpdatedUserDataMessageAsync(User userForUpdate)
        {
            var publishUpdatedUser = mapper.Map<PublishUpdatedUserModel>(userForUpdate);
            publishUpdatedUser.EventType = AppConstants.eventTypeUpdatedUserInfo;
            publishUpdatedUser.Roles = await repository.GetUserRolesAsync(userForUpdate);

            var updatedUserMessage = JsonSerializer.Serialize(publishUpdatedUser);
            publishNewMessage.PublishMessage(updatedUserMessage);
        }
    }
}
