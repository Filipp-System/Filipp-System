using Calculator.BaseRepository;
using Calculator.Controls.Grid;
using Calculator.Models;
using Calculator.Models.DatabaseModels;

namespace Calculator.Client.Data
{
    /// <summary>
    /// Extension class with helpers
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Transfers the new page information over.
        /// </summary>
        /// <param name="helper">The <see cref="PageHelper"/> to use.</param>
        /// <param name="newData">The new data to transfer.</param>
        public static void Refresh(this IPageHelper helper, IPageHelper newData)
        {
            helper.PageSize = newData.PageSize;
            helper.PageItems = newData.PageItems;
            helper.Page = newData.Page;
            helper.TotalItemCount = newData.TotalItemCount;
        }

        /// <summary>
        /// Helper to transfer concurrency information from the repository to the data object.
        /// </summary>
        /// <param name="calculation"> <see cref="Calculator.Models.DatabaseModels.Calculation"/> being resolved.</param>
        /// <param name="repository">The <see cref="WasmRepository"/> holding the concurrency values.</param>
        /// <returns>The <see cref="CalculationConcurrencyResolver"/> instance.</returns>
        public static CalculationConcurrencyResolver ToConcurrencyResolver(this Calculation calculation,
            IBasicRepository<Calculation> repository)
        {
            return new CalculationConcurrencyResolver()
            {
                OriginalCalculation = calculation,
                // todo: set RowVersion here
            };
        }
    }
}
