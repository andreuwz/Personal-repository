namespace Cart.API.Web.AsyncMessageBusServices.EventProcessing
{
    public interface IEventProcessor
    {
        Task ProcessEvent(string message);
    }
}
