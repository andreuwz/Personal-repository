namespace Catalogue.API.DTO.Request
{
    public class PublishedUserModel
    {
        public PublishedUserModel()
        {
            Roles = new List<string>();
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string EventType { get; set; }
        public List<string> Roles { get; set; }
    }
}
