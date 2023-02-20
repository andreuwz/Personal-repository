namespace Catalogue.API.Web.EventProcessing
{
    public interface IEventProcessor
    {
        Task ProcessEventAsync(string message);
        Task ProccessRpcEventAsync(string message);
    }
}