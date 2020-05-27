using System.ComponentModel.DataAnnotations;

namespace Calculator.WebAPI.Models
{
    public class Material
    {
        [Key]
        public int MaterialID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Durability { get; set; }
        [Required]
        public decimal Weight { get; set; }
        [Required]
        public decimal Price { get; set; }

    }
}
