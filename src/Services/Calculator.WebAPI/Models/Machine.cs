using System.ComponentModel.DataAnnotations;

namespace Calculator.WebAPI.Models
{
    public class Machine
    {
        [Key]
        public int MachineID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal PricePerHour { get; set; }
        [Required]
        public decimal PricePerHourLowQuantity { get; set; }
        [Required]
        public decimal SetupTime { get; set; }
        [Required] 
        public decimal LaborCost { get; set; }
        [Required]
        public decimal AcquisitionCost { get; set; }
    }
}
