using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using Calculator.Models;
using Calculator.Models.DatabaseModels;

namespace Calculator.Controls.Grid
{
    public class GridQueryAdapter
    {
        private readonly IEmployeeFilters _controls;

        private readonly Dictionary<EmployeeFilterColumns, Expression<Func<Employee, string>>> _expressions
            = new Dictionary<EmployeeFilterColumns, Expression<Func<Employee, string>>>
            {
                {EmployeeFilterColumns.City, e => e.City},
                {EmployeeFilterColumns.PhoneNumber, e => e.PhoneNumber},
                {EmployeeFilterColumns.LastName, e => e.LastName},
                {EmployeeFilterColumns.Profession, e => e.Profession},
                {EmployeeFilterColumns.Street, e => e.Street},
                {EmployeeFilterColumns.ZipCode, e => e.ZipCode.ToString(CultureInfo.InvariantCulture)}
            };
    }
}
