using Application.ModelValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Application.Users
{
    public class CreateUserModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        [MinLength(5)]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage ="Password is not compatible.")] //8 chars, one digit, one special character
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        [Required]
        [MaxLength(15)]
        [MinLength(5)]
        public string Firstname { get; set; }
        [Required]
        [ObsoleteDateValidation(ErrorMessage ="You cannot enter past dates (check time).")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}
