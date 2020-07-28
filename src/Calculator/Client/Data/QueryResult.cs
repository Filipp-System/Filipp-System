using System.Collections.Generic;
using Calculator.Controls.Grid;
using Calculator.Models;
using Calculator.Models.DatabaseModels;

namespace Calculator.Client.Data
{
    /// <summary>
    /// Result from query request.
    /// </summary>
    public class QueryResult
    {
        /// <summary>
        /// New <see cref="PageHelper"/> information.
        /// </summary>
        public PageHelper PageInfo { get; set; }

        /// <summary>
        /// A page of <see cref="ICollection{Calculation}"/>.
        /// </summary>
        public ICollection<Employee> Employees { get; set; }   
    }
}
