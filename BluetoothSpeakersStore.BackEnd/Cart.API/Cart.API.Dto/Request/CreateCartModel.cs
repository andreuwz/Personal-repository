namespace Cart.API.DTO.Request
{
    public class CreateCartModel
    {
        public Guid CartId { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
