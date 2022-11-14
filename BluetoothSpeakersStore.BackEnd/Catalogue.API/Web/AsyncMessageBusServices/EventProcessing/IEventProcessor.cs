using Catalogue.API.DTO.Request;

namespace Catalogue.API.Web.EventProcessing
{
    public interface IEventProcessor
    {
        Task ProcessEventAsync(string message);
    }
}