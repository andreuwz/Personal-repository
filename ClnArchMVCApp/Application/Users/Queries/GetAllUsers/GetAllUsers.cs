using Application.Interfaces.Persistence;

namespace Application.Users.Queries.GetAllUsers
{
    public class GetAllUsers : IGetAllUsers
    {
        private readonly IUserRepository userRepository;

        public GetAllUsers(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IEnumerable<CreateUserModel> Execute()
        {
            return userRepository.GetAll()
                 .Select(f => new CreateUserModel()
                 {
                     Id = f.Id,
                     Username = f.Username,
                     IsAdmin = f.IsAdmin,
                     CreatedAt = f.CreatedAt,
                     Firstname = f.Firstname,
                 });
        }
    }
}
