using Application.Interfaces.Persistence;
using Domain.Users;

namespace Persistance.Repositories.Users
{
    public class UserRepository : Repository<User>, IUserRepository
    {
    }
}
