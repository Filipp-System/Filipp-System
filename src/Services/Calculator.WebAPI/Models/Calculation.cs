using System.ComponentModel.DataAnnotations;

namespace Calculator.WebAPI.Models
{
    public class Calculation
    {
        [Key] 
        public int CalculationID { get; set; }
        [Required] 
        public string Owner { get; set; }
    }
}
