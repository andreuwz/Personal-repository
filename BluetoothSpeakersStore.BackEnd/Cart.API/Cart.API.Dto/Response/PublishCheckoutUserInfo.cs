namespace Cart.API.DTO.Response
{
    public class PublishCheckoutUserInfo
    {
        public Guid Id { get; set; }
        public string EventType { get; set; } = "AcquireUserBalance";
    }
}
