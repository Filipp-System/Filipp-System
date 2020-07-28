using Calculator.Models.DatabaseModels;

namespace Calculator.Client.Data
{
    /// <summary>
    /// This class helps track concurrency issues for client/server scenarios.
    /// </summary>
    public class EmployeeConcurrencyResolver
    {
        /// <summary>
        /// The latest database version
        /// </summary>
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// The <see cref="Employee"/> being updated.
        /// </summary>
        public Employee OriginalEmployee { get; set; }

        /// <summary>
        /// The <see cref="Employee"/> as it exists in the database.
        /// </summary>
        public Employee DatabaseEmployee { get; set; }
    }
}
