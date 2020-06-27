using System.ComponentModel.DataAnnotations;

namespace Calculator.Model
{
    public class Task
    {
        [Key]
        public int TaskID { get; set; }
        [Required]
        public string State { get; set; }
    }
}
