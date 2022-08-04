using System.ComponentModel.DataAnnotations;

namespace Application.Furnitures
{
    public class FurnitureModel // DTO
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage ="Input consists of unallowed characters.")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Input consists of unallowed characters.")]
        public string Type { get; set; }
        [Required]
        [MaxLength(150)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Input consists of unallowed characters.")]
        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }

    }
}
