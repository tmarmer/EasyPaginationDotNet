using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class LimitOffsetCalculator : IPageCalculator
    {
        public IEnumerable<PageData> GetAllPages(PaginationInfo paginationInfo) =>
            new[]
            {
                StaticLimitOffsetCalculator.GetFirstPage(paginationInfo),
                StaticLimitOffsetCalculator.GetPreviousPage(paginationInfo),
                StaticLimitOffsetCalculator.GetNextPage(paginationInfo),
                StaticLimitOffsetCalculator.GetLastPage(paginationInfo)
            };
    }
}