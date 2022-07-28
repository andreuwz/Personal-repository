using System.ComponentModel.DataAnnotations;

namespace Application.Users
{
    public class UpdateUserModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        [MinLength(5)]
        public string Username { get; set; }
        [Required]
        [MaxLength(15)]
        [MinLength(5)]
        public string Firstname { get; set; }
    }
}
