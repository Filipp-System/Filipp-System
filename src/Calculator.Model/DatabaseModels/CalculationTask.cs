using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calculator.Models.DatabaseModels
{
    public class CalculationTask
    {
        [Key]
        public int CalculationTaskID { get; set; }

        [ForeignKey(nameof(Calculation)), Required]
        public int CalculationID { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime LastSave { get; set; }

        public Calculation Calculation { get; set; }
    }
}
