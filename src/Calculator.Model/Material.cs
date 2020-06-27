using System.ComponentModel.DataAnnotations;

namespace Calculator.Model
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
