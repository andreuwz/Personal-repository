using Application.Interfaces.Persistence;
using Domain.Users;

namespace Application.Users.Queries.GetUser
{
    public class GetUser : IGetUser
    {
        private readonly IUserRepository userRepository;

        public GetUser(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        User IGetUser.GetUser(int id)
        {
            return userRepository.Get(id);
        }
    }
}
