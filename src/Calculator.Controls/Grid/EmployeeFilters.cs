using Calculator.Models;

namespace Calculator.Controls.Grid
{
    public class EmployeeFilters : IEmployeeFilters
    {
        public EmployeeFilters(IPageHelper pageHelper)
        {
            PageHelper = pageHelper;
        }

        public EmployeeFilterColumns FilterColumn { get; set; } = EmployeeFilterColumns.LastName;
        public string FilterText { get; set; }
        public IPageHelper PageHelper { get; set; }
        public bool Loading { get; set; }
        public bool ShowFirstNameFirst { get; set; }
        public bool SortAscending { get; set; }
        public EmployeeFilterColumns SortColumn { get; set; } = EmployeeFilterColumns.LastName;
    } 
}
