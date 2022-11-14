using Identity.API.Domain;
using Identity.API.DTO.Response;

namespace Identity.API.Application.Users.Queries.GetAllUsers
{
    public interface IGetAllUsers
    {
        Task<IEnumerable<GetUserModel>> GetAllUsersAsync();
    }
}