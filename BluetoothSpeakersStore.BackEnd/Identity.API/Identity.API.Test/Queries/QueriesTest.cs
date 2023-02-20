using Identity.API.Domain;

namespace Identity.API.Test.Queries
{
    public class QueriesTest : ApplicationLayerTestBase
    {
        [Fact]
        public async Task GetAllUsers_Returns_Result_Successfully()
        {
            var getAllUsersService = SetupGetAllUsersObject();
            var userList = await getAllUsersService.GetAllUsersAsync();

            Assert.Equal(typeof(List<User>), userList.GetType());
        }
    }
}
