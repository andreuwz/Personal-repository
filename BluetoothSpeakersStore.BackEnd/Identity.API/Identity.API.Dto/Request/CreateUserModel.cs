using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTO.Request
{
    public class CreateUserModel
    {
        [Required(ErrorMessage = "Username field is mandatory.")]
        [MaxLength(15)]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "The field should contain only letters and spaces.")]
        [MaxLength(25)]
        public string Firstname { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "The field should contain only letters and spaces.")]
        [MaxLength(25)]
        public string Lastname { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password is not compatible.")] //8 chars, one digit, one special character
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The entered passwords does not match!")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "The entered email has invalid format.")]
        [MinLength(5)]
        public string Email { get; set; }
    }
}
