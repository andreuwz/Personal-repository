namespace Identity.API.DTO.Response
{
    public class PublishPrincipalUserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string EventType { get; set; }
        public double Balance { get; set; }
        public List<string> Roles { get; set; }
    }
}
