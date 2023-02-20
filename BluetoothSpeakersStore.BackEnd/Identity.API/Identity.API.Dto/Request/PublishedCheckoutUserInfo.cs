namespace Identity.API.DTO.Request
{
    public class PublishedCheckoutUserInfo
    {
        public Guid Id { get; set; }
        public string EventType { get; set; } = "AcquireUserBalance";
    }
}
