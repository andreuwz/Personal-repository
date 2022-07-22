using System.ComponentModel.DataAnnotations;

namespace Application.Furnitures
{
    public class FurnitureModel // DTO
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

    }
}
