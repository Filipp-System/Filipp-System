using System;
using System.Collections.Generic;
using System.Text;
using Calculator.Model;

namespace Calculator.Controls.Grid
{
    public class PageHelper : IPageHelper
    {
        public int DbPage => Page - 1;
        public int Page { get; set; }
        public int PageCount => (TotalItemCount + PageSize - 1) / PageSize;
        public int NextPage => Page < PageCount ? Page + 1 : Page;
        public bool HasNextPage => Page < PageCount;
        public int PreviousPage => Page > 1 ? Page - 1 : Page;
        public bool HasPreviousPage => Page > 1;
        public int PageItems { get; set; }
        public int PageSize { get; set; } = 20;
        public int Skip => PageSize * DbPage;
        public int TotalItemCount { get; set; }
    }
}
