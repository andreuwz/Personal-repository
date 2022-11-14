using AutoMapper;
using Identity.API.Application.Users.Validations;
using Identity.API.Common.Constants;
using Identity.API.Domain;
using Identity.API.DTO.Response;
using Identity.API.Persistence.RepositoryContract;
using Identity.API.Web.AsyncMessageBusServices.PublishMessages;
using System.Text.Json;

namespace Identity.API.Application.Users.Commands.DeleteUser
{
    internal class DeleteUser : IDeleteUser
    {
        private readonly IIdentityRepository repository;
        private readonly IUserValidations userValidations;
        private readonly IMapper mapper;
        private readonly IPublishNewMessage publishNewMessage;

        public DeleteUser(IIdentityRepository repository, IUserValidations userValidations, 
            IMapper mapper, IPublishNewMessage publishNewMessage)
        {
            this.repository = repository;
            this.userValidations = userValidations;
            this.mapper = mapper;
            this.publishNewMessage = publishNewMessage;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var userForDelete = await repository.FindByIdAsync(id.ToString());
            userValidations.EnsureUserExists(userForDelete);

            PublishUpdatedUserDataMessageAsync(userForDelete);
            await repository.DeleteUserAsync(userForDelete);

            return true;
        }

        private void PublishUpdatedUserDataMessageAsync(User userForUpdate)
        {
            var publishUpdatedUser = mapper.Map<PublishUpdatedUserModel>(userForUpdate);
            publishUpdatedUser.EventType = AppConstants.eventTypeUserDelete;

            var updatedUserMasage = JsonSerializer.Serialize(publishUpdatedUser);
            publishNewMessage.PublishMessage(updatedUserMasage);
        }
    }
}
