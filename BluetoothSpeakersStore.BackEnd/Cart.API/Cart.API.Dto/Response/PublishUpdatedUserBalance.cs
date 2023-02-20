using Cart.API.Common;

namespace Cart.API.DTO.Response
{
    public class PublishUpdatedUserBalance
    {
        public Guid Id { get; set; }
        public double Balance { get; set; }
        public string EventType { get; set; } = AppConstants.eventTypeUpdatedUserBalance;
    }
}
