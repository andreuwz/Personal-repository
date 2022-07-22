using Domain.Common;
using Domain.Furnitures;
using System.ComponentModel.DataAnnotations;

namespace Domain.Users
{
    public class User : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public List<Furniture> Furnitures { get; set; }
    }
}
