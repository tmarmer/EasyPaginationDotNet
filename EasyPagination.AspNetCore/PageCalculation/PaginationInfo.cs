using System;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class PaginationInfo
    {
        public readonly IPaginationParams PaginationParams;
        public readonly Uri BaseUri;
        public readonly int ItemCount;
        public readonly int? TotalItems;

        public PaginationInfo(IPaginationParams paginationParams, Uri baseUri, int itemCount, int? totalItems)
        {
            PaginationParams = paginationParams;
            BaseUri = baseUri;
            ItemCount = itemCount;
            TotalItems = totalItems;
        }
    }
}