using System.Collections.Generic;
using Calculator.Controls.Grid;
using Calculator.Model;

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
        /// A page of <see cref="ICollection{Employee}"/>.
        /// </summary>
        public ICollection<Employee> Employees { get; set; }   
    }
}
