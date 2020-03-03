using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public interface IPageCalculator
    {
        IEnumerable<PageData> GetAllPages(PaginationInfo paginationInfo);
    }
}