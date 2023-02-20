namespace Identity.API.DTO.Response
{
    public class LoginOutputModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Roles { get; set; }
    }
}
