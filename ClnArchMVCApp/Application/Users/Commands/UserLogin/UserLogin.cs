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
        public User Execute(string username, string password)
        {

            var foundUser = userRepository.Login(username, password);

            if (foundUser != null)
            {
                return foundUser;
            }

            return null;
            
        }
    }
}
