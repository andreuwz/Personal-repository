using Application.Interfaces.Persistence;

namespace Application.Users.Queries.GetUser
{
    public class GetUser : IGetUser
    {
        private readonly IUserRepository userRepository;

        public GetUser(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public CreateUserModel Execute(int id)
        {
            var user = userRepository.Get(id);  

            return new CreateUserModel()
            {
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                Firstname = user.Firstname,
                IsAdmin = user.IsAdmin,
                Id = user.Id
            };
        }
    }
}
