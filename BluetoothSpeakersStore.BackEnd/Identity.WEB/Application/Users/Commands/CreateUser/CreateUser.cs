using AutoMapper;
using Identity.API.Application.Users.Validations;
using Identity.API.Domain;
using Identity.API.DTO.Request;
using Identity.API.Persistence.RepositoryContract;

namespace Identity.API.Application.Users.Commands.CreateUser
{
    internal class CreateUser : ICreateUser
    {
        private readonly IIdentityRepository repository;
        private readonly IMapper mapper;
        private readonly IUserValidations userValidation;

        public CreateUser(IIdentityRepository repository, IMapper mapper, IUserValidations userValidation)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.userValidation = userValidation;
        }

        public async Task<bool> CreateUserAsync(CreateUserModel userModel)
        {
            await userValidation.EnsureCreatedUniqueUsernameAndEmailAsync(userModel);

            var newUser = mapper.Map<User>(userModel);
            newUser.EmailConfirmed = true; //*
            newUser.CreatedAt = DateTime.Now.Date;
            newUser.ModifiedAt = DateTime.Now.Date;
       
            await repository.CreateUserAsync(newUser, userModel.Password);
            await repository.AddUserToRoleAsync(newUser, "RegularUser");

            return true;
        }
    }
}
