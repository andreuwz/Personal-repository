using Application.Interfaces.Persistence;
using Domain.Users;

namespace Persistance.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
