namespace Cart.API.Web.AsyncMessageBusServices.Rpc
{
    public interface IUserRpcClient
    {
        Task<double> AcquireUserBalance(Guid userId);
    }
}