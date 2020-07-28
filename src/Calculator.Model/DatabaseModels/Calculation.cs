using System.ComponentModel.DataAnnotations;

namespace Calculator.Models.DatabaseModels
{
    public class Calculation
    {
        [Key] 
        public int CalculationID { get; set; }
        [Required] 
        public string Owner { get; set; }
    }
}
