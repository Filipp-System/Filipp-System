namespace Calculator.Model
{
    public interface IPageHelper
    {
        /// <summary>
        /// Current Page 0-based
        /// </summary>
        int DbPage { get; }
        /// <summary>
        /// Current Page 1-based
        /// </summary>
        int Page { get; set; }
        int PageCount { get; }
        int NextPage { get; }
        bool HasNextPage { get; }
        int PreviousPage { get; }
        bool HasPreviousPage { get; }
        int ItemsOnCurrentPage { get; set; }
        int ItemsPerPage { get; set; }
        int HowManyItemsToSkip { get; }
        int TotalItemCountBasedOnFilter { get; set; }
    }
}
