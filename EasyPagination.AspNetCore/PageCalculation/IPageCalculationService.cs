using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public interface IPageCalculationService
    {
        IEnumerable<PageData> Calculate(IPaginationParams paginationParams, Uri baseUri, int itemCount,
            int? totalItems);
    }
}