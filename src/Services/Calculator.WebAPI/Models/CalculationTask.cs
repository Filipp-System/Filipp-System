using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator.WebAPI.Models
{
    public class CalculationTask
    {
        [Key]
        public int CalculationTaskID { get; set; }
        [ForeignKey(nameof(Models.Task))] 
        public int TaskID { get; set; }
        [ForeignKey(nameof(Calculation))]
        public int CalculationID { get; set; }

        public Task Task { get; set; }
        public Calculation Calculation { get; set; }
    }
}
