using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public interface IPageCalculator
    {
        IEnumerable<PageData> GetAllPages(IPaginationParams paginationParams, Uri baseUri, int itemCount,
            int? totalItems);
    }
}