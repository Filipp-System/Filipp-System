using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Calculator.Models.DatabaseModels
{
    public class FilippSystemApplicationInformation
    {
        [Key, Required]
        public int FilippSystemApplicationInformationID { get; set; }

        [Required]
        public string ApplicationVersion { get; set; }

        [Required]
        public string ApplicationAssemblyVersion { get; set; }
    }
}
