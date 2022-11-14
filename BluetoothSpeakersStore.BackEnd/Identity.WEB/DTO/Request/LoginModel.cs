using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTO.Request
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username field is mandatory.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password field is mandatory.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
