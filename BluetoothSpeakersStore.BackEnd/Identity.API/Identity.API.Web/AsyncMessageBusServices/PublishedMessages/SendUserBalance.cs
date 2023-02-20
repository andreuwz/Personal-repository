using Identity.API.Persistence.RepositoryContract;

namespace Identity.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public class SendUserBalance : ISendUserBalance
    {
        private readonly IIdentityRepository identityRepository;

        public SendUserBalance(IIdentityRepository identityRepository)
        {
            this.identityRepository = identityRepository;
        }

        public async Task<double> ExtractAndSendUserBalance(Guid userId)
        {
            var formattedUserId = userId.ToString();
            var user = await identityRepository.FindByIdAsync(formattedUserId);
            return user.Balance;
        }
    }
}
