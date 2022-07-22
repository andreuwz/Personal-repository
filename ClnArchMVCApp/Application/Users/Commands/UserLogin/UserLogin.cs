using Application.Interfaces.Persistence;

namespace Application.Users.Commands.UserLogin
{
    public class UserLogin : IUserLogin
    {
        public IUserRepository userRepository;
        public UserLogin(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public bool Execute(int id)
        {
            var foundUser = userRepository.Get(id);

            if (foundUser != null)
            {
                return true;
            }

            return false;
        }
    }
}
