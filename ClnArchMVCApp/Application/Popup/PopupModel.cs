using Domain.Common;

namespace Application.Popup
{
    public class PopupModel : IEntity
    {
        public string Message { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Root { get; set; }
        public int Id { get; set; }
    }
}
