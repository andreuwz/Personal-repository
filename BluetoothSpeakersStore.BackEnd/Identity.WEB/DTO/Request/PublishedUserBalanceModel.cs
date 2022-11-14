namespace Identity.API.DTO.Request
{
    public class PublishedUserBalanceModel
    {
        public Guid Id { get; set; }
        public double Balance { get; set; }
        public string EventType { get; set; }
    }
}
