namespace Identity.API.Web.AsyncMessageBusServices.EventProcessing
{
    public interface IEventProcessor
    {
        Task ProcessEventAsync(string message);
    }
}