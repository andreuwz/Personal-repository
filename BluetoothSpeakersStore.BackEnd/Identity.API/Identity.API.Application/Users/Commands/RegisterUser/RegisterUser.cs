using AutoMapper;
using Identity.API.Application.Users.Validations;
using Identity.API.Domain;
using Identity.API.DTO.Request;
using Identity.API.Persistence.RepositoryContract;
using System.Text.Json;

namespace Identity.API.Application.Users.Commands.RegisterUser
{
    public class RegisterUser : IRegisterUser
    {
        private readonly IIdentityRepository identityRepository;
        private readonly IUserValidations userValidation;
        private readonly IMapper mapper;

        public RegisterUser(IIdentityRepository identityRepository,
            IUserValidations userValidation, IMapper mapper)
        {
            this.identityRepository = identityRepository;
            this.userValidation = userValidation;
            this.mapper = mapper;
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
            
            var roleList = await identityRepository.GetUserRolesAsync(newUser);
            var resultRoleList = JsonSerializer.Serialize(roleList);

            return resultRoleList;
        }
    }
}
