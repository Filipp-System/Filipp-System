using System;

namespace Calculator.Model
{
    public interface IFilters<T> where T : Enum
    {
        T FilterColumn { get; set; }
        string FilterText { get; set; }
        IPageHelper PageHelper { get; set; }
        bool Loading { get; set; }
        bool SortAscending { get; set; }
        T SortColumn { get; set; }
    }
}
