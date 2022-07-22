using Domain.Common;
using Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace Domain.Furnitures
{
    public class Furniture : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        public List<User> Users { get; set; }
    }
}
