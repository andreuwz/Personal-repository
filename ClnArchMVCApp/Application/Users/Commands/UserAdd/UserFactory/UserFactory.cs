using Application.Interfaces.Persistence;
using Domain.Users;

namespace Application.Users.Commands.UserAdd.UserFactory
{
    public class UserFactory : IUserFactory
    {
        private readonly IUserRepository userRepository;

        public UserFactory(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User Execute(string username, string password, string firstname, bool isAdmin, DateTime createdAt)
        {
            User newUser = new User()
            {
                Username = username,
                Password = password,
                Firstname = firstname,
                IsAdmin = isAdmin,
                CreatedAt = createdAt
            };

            userRepository.Add(newUser);
            return newUser; 
        }
    }
}
