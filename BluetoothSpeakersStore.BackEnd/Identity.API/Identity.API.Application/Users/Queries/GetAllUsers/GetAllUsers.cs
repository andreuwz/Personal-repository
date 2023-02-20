using AutoMapper;
using Identity.API.DTO.Response;
using Identity.API.Persistence.RepositoryContract;

namespace Identity.API.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsers : IGetAllUsers
    {
        private readonly IIdentityRepository repository;
        private readonly IMapper mapper;

        public GetAllUsers(IIdentityRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GetUserModel>> GetAllUsersAsync()
        {
            var userList = await repository.GetAllUsersAsync();
            var outputUserList = mapper.Map<List<GetUserModel>>(userList);

            foreach (var outputUser in outputUserList)
            {
                var getMatchingUser = userList.FirstOrDefault(u => u.Id == outputUser.Id.ToString());
                var userRoles = await repository.GetUserRolesAsync(getMatchingUser);
                outputUser.UserRoles = userRoles;
            }
            return outputUserList;
        }
    }
}
