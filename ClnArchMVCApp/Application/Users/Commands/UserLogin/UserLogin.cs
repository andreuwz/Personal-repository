using Application.Interfaces.Persistence;
using Domain.Users;

namespace Application.Users.Commands.UserLogin
{
    public class UserLogin : IUserLogin
    {
        public IUserRepository userRepository;
        public UserLogin(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public CreateUserModel Execute(string username, string password)
        {
            if (username == null || password == null)
            {
                throw new ArgumentNullException();
            }

            var foundUser = userRepository.Login(username, password);

            if (foundUser != null)
            {
                return new CreateUserModel()
                {
                    Username = foundUser.Username,
                    Firstname = foundUser.Firstname,
                    IsAdmin = foundUser.IsAdmin,
                };
            }

            return null;
        }
    }
}
