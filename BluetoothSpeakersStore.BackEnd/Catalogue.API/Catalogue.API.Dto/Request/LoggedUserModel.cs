namespace Catalogue.API.DTO.Request
{
    public class LoggedUserModel
    {
        public LoggedUserModel()
        {
            Roles = new List<string>();
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
