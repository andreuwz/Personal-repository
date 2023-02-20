namespace Cart.API.Web.AsyncMessageBusServices.Rpc
{
    public interface IRpcHelperService
    {
        void DetermineResponseEventType(string rpcProductResponse);
        void ValidateBuyerBalance(double balance, double totalSum);
    }
}