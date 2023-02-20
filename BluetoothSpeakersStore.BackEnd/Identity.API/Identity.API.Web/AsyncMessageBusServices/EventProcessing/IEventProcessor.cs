namespace Identity.API.Web.AsyncMessageBusServices.EventProcessing
{
    public interface IEventProcessor
    {
        Task ProcessEventAsync(string message);
        Task<double> ProcessRpcEventAsync(string message);
    }
}