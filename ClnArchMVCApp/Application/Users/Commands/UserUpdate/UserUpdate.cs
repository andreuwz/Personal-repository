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

        public CreateUserModel Execute(UpdateUserModel user)
        {
            var foundUser = userRepoistory.Get(user.Id);
            foundUser.Firstname = user.Firstname;
            foundUser.Username = user.Username;

            userRepoistory.Update(foundUser);   

            return new CreateUserModel()
            {
                Username = foundUser.Username,
                Firstname = foundUser.Firstname
            };
        }
    }
}
