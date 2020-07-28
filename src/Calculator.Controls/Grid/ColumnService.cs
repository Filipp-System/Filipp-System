using System;
using System.Collections.Generic;
using System.Text;
using Calculator.Models;

namespace Calculator.Controls.Grid
{
    public class ColumnService
    {
        private readonly Dictionary<EmployeeFilterColumns, string> _columnMappings =
            new Dictionary<EmployeeFilterColumns, string>
            {
                {EmployeeFilterColumns.City, "d-none d-sm-block col-lg-1 col-sm-3"},
                {EmployeeFilterColumns.LastName, "col-8 col-lg-2 col-sm-3"},
                {EmployeeFilterColumns.PhoneNumber, "d-none d-sm-block col-lg-2 col-sm-2"},
                {EmployeeFilterColumns.Profession, "d-none d-sm-block col-sm-1"},
                {EmployeeFilterColumns.Street, "d-none d-lg-block col-lg-3"},
                {EmployeeFilterColumns.ZipCode, "d-none d-sm-block col-sm-2"}
            };

        /// <summary>
        /// Left edit
        /// </summary>
        public string EditColumn => "col-4 col-sm-1";

        /// <summary>
        /// Delete confirmation
        /// </summary>
        public string DeleteConfirmation => "col-lg-9 col-sm-8";

        /// <summary>
        /// Get attributes for column
        /// </summary>
        /// <param name="column">The <see cref="EmployeeFilterColumns"/> to reference.</param>
        /// <returns>A <see cref="string"/> representing the classes.</returns>
        public string GetClassForColumn(EmployeeFilterColumns column) => _columnMappings[column];
    }
}
