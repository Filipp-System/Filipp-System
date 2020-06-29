using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator.Model;

namespace Calculator.Server.Controllers
{
    /// <summary>
    /// Simple implementation of <see cref="IEmployeeFilters"/> for
    /// serialization across REST endpoints.
    /// </summary>
    public class EmployeeFilter : IEmployeeFilters
    {
        public EmployeeFilter()
        {
            //PageHelper = new PageHelper
        }
        public EmployeeFilterColumns FilterColumn { get; set; }
        public string FilterText { get; set; }
        public IPageHelper PageHelper { get; set; }
        public bool Loading { get; set; }
        public bool ShowFirstNameFirst { get; set; }
        public bool SortAscending { get; set; }
        public EmployeeFilterColumns SortColumn { get; set; }
    }
}
