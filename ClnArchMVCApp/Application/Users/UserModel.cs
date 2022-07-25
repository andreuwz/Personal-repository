using System.ComponentModel.DataAnnotations;

namespace Application.Users
{
    public class UserModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        [Required]
        [MaxLength(15)]
        public string Firstname { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
