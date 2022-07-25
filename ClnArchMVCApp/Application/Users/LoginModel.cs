using System.ComponentModel.DataAnnotations;

namespace Application.Users
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
