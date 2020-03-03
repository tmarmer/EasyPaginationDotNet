using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class LimitOffsetCalculator : IPageCalculator
    {
        public IEnumerable<PageData> GetAllPages(IPaginationParams paginationParams, Uri baseUri, int itemCount, int? totalItems) =>
            new[]
            {
                StaticLimitOffsetCalculator.GetFirstPage(paginationParams, baseUri, itemCount, totalItems),
                StaticLimitOffsetCalculator.GetPreviousPage(paginationParams, baseUri, itemCount, totalItems),
                StaticLimitOffsetCalculator.GetNextPage(paginationParams, baseUri, itemCount, totalItems),
                StaticLimitOffsetCalculator.GetLastPage(paginationParams, baseUri, itemCount, totalItems)
            };
    }
}