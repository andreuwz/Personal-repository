using AutoMapper;
using Identity.API.Application.Users.Validations;
using Identity.API.Common.Constants;
using Identity.API.Domain;
using Identity.API.DTO.Request;
using Identity.API.DTO.Response;
using Identity.API.Persistence.RepositoryContract;
using Identity.API.Web.AsyncMessageBusServices.PublishMessages;
using System.Text.Json;

namespace Identity.API.Application.Users.Commands.RegisterUser
{
    internal class RegisterUser : IRegisterUser
    {
        private readonly IIdentityRepository identityRepository;
        private readonly IUserValidations userValidation;
        private readonly IMapper mapper;
        private readonly IPublishNewMessage publishNewMessage;

        public RegisterUser(IIdentityRepository identityRepository,
            IUserValidations userValidation, IMapper mapper, IPublishNewMessage publishNewMessage)
        {
            this.identityRepository = identityRepository;

            this.userValidation = userValidation;
            this.mapper = mapper;
            this.publishNewMessage = publishNewMessage;
        }

        public async Task<string> RegisterNewUserAsync(RegisterUserModel userModel)
        {
            await userValidation.EnsureRegisteredUniqueUsernameAndEmailAsync(userModel);

            var newUser = mapper.Map<User>(userModel);
            newUser.EmailConfirmed = true; //*
            newUser.CreatedAt = DateTime.Now.Date;
            newUser.ModifiedAt = DateTime.Now.Date;

            await identityRepository.CreateUserAsync(newUser, userModel.Password);
            await identityRepository.AddUserToRoleAsync(newUser, "RegularUser");

            await PublishLoggedUserInformationAsync(newUser);
            
            var roleList = await identityRepository.GetUserRolesAsync(newUser);
            var resultRoleList = JsonSerializer.Serialize(roleList);

            return resultRoleList;
        }

        private async Task PublishLoggedUserInformationAsync(User newUser)
        {
            var publishedUser = mapper.Map<PublishPrincipalUserModel>(newUser);
            var loginUserRoles = await identityRepository.GetUserRolesAsync(newUser);
            publishedUser.Roles = loginUserRoles;
            publishedUser.EventType = AppConstants.eventTypePublishLoggedUser;

            var loggedUserMessage = JsonSerializer.Serialize(publishedUser);
            publishNewMessage.PublishMessage(loggedUserMessage);
        }
    }
}
