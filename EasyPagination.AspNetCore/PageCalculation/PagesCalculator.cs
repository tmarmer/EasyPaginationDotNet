using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class PagesCalculator : IPageCalculator
    {
        public IEnumerable<PageData> GetAllPages(PaginationInfo paginationInfo) =>
            new[]
            {
                StaticPagesCalculator.GetFirstPage(paginationInfo),
                StaticPagesCalculator.GetPreviousPage(paginationInfo),
                StaticPagesCalculator.GetNextPage(paginationInfo),
                StaticPagesCalculator.GetLastPage(paginationInfo)
            };
    }
}