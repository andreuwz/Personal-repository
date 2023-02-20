namespace Cart.API.DTO.Response
{
    public class GetUserCartModel
    {
        public Guid CartId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatorName { get; set; }
        public double TotalSum { get; set; }
    }
}
