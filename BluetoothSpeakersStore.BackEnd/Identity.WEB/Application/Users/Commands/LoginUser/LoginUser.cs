using AutoMapper;
using Identity.API.Application.Users.Validations;
using Identity.API.Common.Constants;
using Identity.API.Domain;
using Identity.API.DTO.Response;
using Identity.API.Persistence.RepositoryContract;
using Identity.API.Web.AsyncMessageBusServices.PublishMessages;
using System.Text.Json;

namespace Identity.API.Application.Users.Commands.LoginUser
{
    internal class LoginUser : ILoginUser
    {
        private readonly IIdentityRepository identityRepository;
        private readonly IUserValidations userValidation;
        private readonly IMapper mapper;
        private readonly IPublishNewMessage publishNewMessage;
        private readonly IHttpClientFactory clientFactory;

        public LoginUser(IIdentityRepository identityRepository, IUserValidations userValidation, 
            IMapper mapper, IPublishNewMessage publishNewMessage, IHttpClientFactory clientFactory)
        {
            this.identityRepository = identityRepository;
            this.userValidation = userValidation;
            this.mapper = mapper;
            this.publishNewMessage = publishNewMessage;
            this.clientFactory = clientFactory;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var loginUser = await identityRepository.FindByNameAsync(username);
            userValidation.EnsureUserExists(loginUser);
            await userValidation.EnsureLoginIsPossible(loginUser, password);

            var roleList = await identityRepository.GetUserRolesAsync(loginUser);
            var resultRoleList = JsonSerializer.Serialize(roleList);
            return resultRoleList;
        }
    }
}
