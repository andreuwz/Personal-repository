namespace Cart.API.DTO.Request
{
    public class LoggedUserModel
    {
        public LoggedUserModel()
        {
            Roles = new List<string>();
        }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public double Balance { get; set; }
        public List<string> Roles { get; set; }
    }
}
