using Identity.API.Application.Exceptions;
using Identity.API.Common.Constants;
using Identity.API.Domain;
using Identity.API.DTO.Request;
using Identity.API.Persistence.RepositoryContract;
using System.Security.Claims;

namespace Identity.API.Application.Users.Validations
{
    internal class UserValidations : IUserValidations
    {
        private readonly IIdentityRepository identityRepository;
        private readonly ISignInRepository signInRepository;

        public UserValidations(IIdentityRepository identityRepository, ISignInRepository signInRepository)
        {
            this.identityRepository = identityRepository;
            this.signInRepository = signInRepository;
        }
        public bool EnsureUserExists(User user)
        {
            var condition = user == null ? throw new UserNotFoundException(AppConstants.userNotFound) : true;
            return condition;
        }

        public async Task<bool> EnsureCreatedUniqueUsernameAndEmailAsync(CreateUserModel userModel)
        {
            var duplicateUsername = await identityRepository.FindByNameAsync(userModel.UserName);
            var duplicateEmail = await identityRepository.FindUserByEmailAsync(userModel.Email);

            var condition = duplicateUsername != null || duplicateEmail != null ?
                throw new DuplicateCredentialsException(AppConstants.duplicateCredentials) : true;

            return condition;
        }

        public async Task<bool> EnsureEditedEmailSameAsUserModelAsync(User user, string email)
        {
            var originalEmail = user.Email;

            if (email == originalEmail)
            {
                return true;
            }
            return await EnsureEditedEmailIsUniqeAsync(email);
        }

        public async Task<bool> EnsureEditedEmailIsUniqeAsync(string email)
        {
            var duplicateEmail = await identityRepository.FindUserByEmailAsync(email);

            var condition = duplicateEmail != null ? throw new DuplicateCredentialsException(AppConstants.duplicateEmail) : true;

            return condition;
        }

        public async Task<bool> EnsureEditedUsernameSameAsUserModelAsync(User user, string username)
        {
            var originalUsername = user.UserName;

            if (username == originalUsername)
            {
                return true;
            }

            return await EnsureEditedUsernameIsUniqueAsync(username);
        }

        public async Task<bool> EnsureEditedUsernameIsUniqueAsync(string username)
        {
            var duplicateUsername = await identityRepository.FindByNameAsync(username);

            var condition = duplicateUsername != null ? throw new DuplicateCredentialsException(AppConstants.duplicateUsername) : true;

            return condition;
        }

        public async Task<bool> EnsureUserNotInRoleAsync(string id, string role)
        {
            var isInRole = await identityRepository.IsUserInRoleAsync(id, role);
            var condition = isInRole ? throw new UserRolesException(AppConstants.userInRole) : true;

            return condition;
        }

        public async Task<bool> EnsureUserInRoleAsync(string id, string role)
        {
            var isInRole = await identityRepository.IsUserInRoleAsync(id, role);
            var condition = isInRole ? true : throw new UserRolesException(AppConstants.userNotInRole);

            return condition;
        }

        public bool EnsureUserNotSignedIn(ClaimsPrincipal user)
        {
            var isUserSignedIn = signInRepository.IsUserSignedIn(user);
            return isUserSignedIn ? throw new UserSessionException(AppConstants.sessionLoggedIn) : true;
        }

        public async Task<bool> EnsureRegisteredUniqueUsernameAndEmailAsync(RegisterUserModel userModel)
        {
            var duplicateUsername = await identityRepository.FindByNameAsync(userModel.UserName);
            var duplicateEmail = await identityRepository.FindUserByEmailAsync(userModel.Email);

            var condition = duplicateUsername != null || duplicateEmail != null ?
                throw new DuplicateCredentialsException(AppConstants.duplicateCredentials) : true;

            return condition;
        }

        public async Task<bool> EnsureLoginIsPossible(User user, string password)
        {
            var loginModel = await signInRepository.IsLoginPossible(user, password);
            return loginModel.Succeeded ? true : throw new UserNotFoundException(AppConstants.userNotFound);
        }

        public async Task<bool> EnsureHttpAuthHeaderExists(string httpHeader)
        {
            return string.IsNullOrEmpty(httpHeader) ? throw new UserSessionException(AppConstants.notLoggedIn) : true;
        }
    }
}
