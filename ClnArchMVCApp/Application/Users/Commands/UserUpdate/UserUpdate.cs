using Application.Interfaces.Persistence;
using Domain.Users;

namespace Application.Users.Commands.UserUpdate
{
    public class UserUpdate : IUserUpdate
    {
        private readonly IUserRepository userRepoistory;

        public UserUpdate(IUserRepository userRepoistory)
        {
            this.userRepoistory = userRepoistory;
        }

        public User Execute(User user)
        {
            var foundUser = userRepoistory.Get(user.Id);
            foundUser.Firstname = user.Firstname;
            foundUser.Username = user.Username;
            foundUser.Password = user.Password;

            userRepoistory.Update(foundUser);
            return foundUser;
        }
    }
}
