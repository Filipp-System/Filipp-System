using System.ComponentModel.DataAnnotations;

namespace Calculator.Models.DatabaseModels
{
    public class Employee
    {
        [Key, Required] public int EmployeeID { get; set; }

        [Required] 
        public string FirstName { get; set; }

        [Required] 
        public string LastName { get; set; }

        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 digits.")]
        public string PhoneNumber { get; set; }

        [Required]
        public string City { get; set; }
        [Required] 
        public string Street { get; set; }
        [Required] 
        public string HouseNumber { get; set; }
        public string HouseNumberExtension { get; set; }

        [Required,
         RegularExpression(@"^(?!01000|99999)(0[1-9]\d{3}|[1-9]\d{4})$",
             ErrorMessage = "Enter a valid format from 01001 to 99998")]
        public int ZipCode { get; set; }

        [Required] 
        public string Profession { get; set; }

        public decimal Salary { get; set; }

        public string UserName { get; set; }
    }
}
