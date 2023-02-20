using System.ComponentModel.DataAnnotations;

namespace Catalogue.API.DTO.Request
{
    public class CreateProductModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Enter currency number in proper format.")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Required]
        [Range(0,int.MaxValue, ErrorMessage = "Enter proper positive number for quantity.")]
        public int Quantity { get; set; }
       
    }
}
