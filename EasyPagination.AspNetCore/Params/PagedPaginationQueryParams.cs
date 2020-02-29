using System;
using Microsoft.AspNetCore.Mvc;

namespace EasyPagination.AspNetCore.Params
{
    public class PagedPaginationQueryParams : PageNumberPaginationParams
    {
        [FromQuery(Name = "pageSize")] public override int PageSize { get; set; }
        [FromQuery(Name = "page")] public override int Offset { get; set; }
    }
}