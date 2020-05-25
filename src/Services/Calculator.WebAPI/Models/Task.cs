using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Calculator.WebAPI.Models
{
    public class Task
    {
        [Key]
        public int TaskID { get; set; }
        [Required]
        public string State { get; set; }
    }
}
