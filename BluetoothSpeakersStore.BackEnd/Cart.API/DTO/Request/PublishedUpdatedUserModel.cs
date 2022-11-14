namespace Cart.API.DTO.Request
{
    public class PublishedUpdatedUserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string EventType { get; set; }
        public double Balance { get; set; }
        public List<string> Roles { get; set; }
    }
}
