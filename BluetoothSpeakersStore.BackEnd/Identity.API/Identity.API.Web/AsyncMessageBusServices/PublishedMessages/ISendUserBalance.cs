namespace Identity.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public interface ISendUserBalance
    {
        Task<double> ExtractAndSendUserBalance(Guid userId);
    }
}