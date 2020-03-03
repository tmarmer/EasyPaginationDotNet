using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public interface IPageCalculationService
    {
        IEnumerable<PageData> Calculate(PaginationInfo paginationInfo);
    }
}