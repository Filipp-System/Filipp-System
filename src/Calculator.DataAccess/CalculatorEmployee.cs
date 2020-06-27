using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Calculator.Model;

namespace Calculator.DataAccess
{
    public class CalculatorEmployee 
    {
        [Key]
        [Required]
        public int CalculatorEmployeeID { get; set; }

        [Required]
        public DateTimeOffset EventTime { get; set; } = DateTimeOffset.UtcNow;

        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public string User { get; set; }
        [Required] 
        public string Action { get; set; }

        public string Changes { get; set; }
        [NotMapped]
        public Employee EmployeeReference { get; set; }
    }
}
