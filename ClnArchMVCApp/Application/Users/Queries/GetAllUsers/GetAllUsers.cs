using Application.Interfaces.Persistence;
using Domain.Users;

namespace Application.Users.Queries.GetAllUsers
{
    public class GetAllUsers : IGetAllUsers
    {
        private readonly IUserRepository userRepository;

        public GetAllUsers(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IEnumerable<User> Execute()
        {
            return userRepository.GetAll();
        }
    }
}
