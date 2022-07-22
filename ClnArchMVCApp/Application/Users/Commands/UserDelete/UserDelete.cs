using Application.Interfaces.Persistence;

namespace Application.Users.Commands.UserDelete
{
    public class UserDelete : IUserDelete
    {
        private readonly IUserRepository userRepository;

        public UserDelete(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public bool Execute(int id)
        {
            var deleteUser = userRepository.Get(id);

            if (deleteUser.IsAdmin)
            {
                throw new Exception();
            }

            userRepository.Delete(deleteUser);
            return true;
        }
    }
}
