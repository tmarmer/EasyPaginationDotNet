using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class PagesCalculator : IPageCalculator
    {
        public IEnumerable<PageData> GetAllPages(IPaginationParams paginationParams, Uri baseUri, int itemCount, int? totalItems) =>
            new[]
            {
                StaticPagesCalculator.GetFirstPage(paginationParams, baseUri, itemCount, totalItems),
                StaticPagesCalculator.GetPreviousPage(paginationParams, baseUri, itemCount, totalItems),
                StaticPagesCalculator.GetNextPage(paginationParams, baseUri, itemCount, totalItems),
                StaticPagesCalculator.GetLastPage(paginationParams, baseUri, itemCount, totalItems)
            };
    }
}