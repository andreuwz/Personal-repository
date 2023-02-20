using AutoMapper;
using Identity.API.Application.Users.Queries.GetAllUsers;
using Identity.API.Domain;
using Identity.API.Persistence.RepositoryContract;
using Moq;

namespace Identity.API.Test
{
    public class ApplicationLayerTestBase
    {
       protected IIdentityRepository SetupIdentityRepository()
        {
            var mockIdentityRepository = new Mock<IIdentityRepository>();
            mockIdentityRepository.Setup(r => r.GetAllUsersAsync())
                .ReturnsAsync(new List<User>());

            return mockIdentityRepository.Object;
        }

        protected IMapper SetupAutoMapper()
        {
            var mockMapper = new Mock<IMapper>();
           

            return mockMapper.Object;
        }

        protected IGetAllUsers SetupGetAllUsersObject()
        {
            var mapper = SetupAutoMapper();
            var identityRepository = SetupIdentityRepository();

            return new GetAllUsers(identityRepository,mapper);
        }
    }
}