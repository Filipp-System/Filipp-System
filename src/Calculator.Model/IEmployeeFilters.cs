namespace Calculator.Model
{
    /// <summary>
    /// Interface for filtering
    /// </summary>
    public interface IEmployeeFilters
    {
        /// <summary>
        /// The <see cref="EmployeeFilterColumns"/> being filtered on.
        /// </summary>
        EmployeeFilterColumns FilterColumn { get; set; }

        /// <summary>
        /// The filter text
        /// </summary>
        string FilterText { get; set; }

        /// <summary>
        /// Paging state in <see cref="PageHelper"/>.
        /// </summary>
        IPageHelper PageHelper { get; set; }

        /// <summary>
        /// Loading new set of data
        /// </summary>
        bool Loading { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the name is rendered first name first.
        /// </summary>
        bool ShowFirstNameFirst { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the sort is ascending or descending.
        /// </summary>
        bool SortAscending { get; set; }

        /// <summary>
        /// The <see cref="EmployeeFilterColumns"/> being sorted.
        /// </summary>
        EmployeeFilterColumns SortColumn { get; set; }
    }
}
