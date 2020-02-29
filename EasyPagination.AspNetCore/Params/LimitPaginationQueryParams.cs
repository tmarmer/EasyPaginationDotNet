using System;
using Microsoft.AspNetCore.Mvc;

namespace EasyPagination.AspNetCore.Params
{
    public class LimitPaginationQueryParams : LimitOffsetPaginationParams
    {
        [FromQuery(Name = "limit")] public override int PageSize { get; set; }
        [FromQuery(Name = "offset")] public override int Offset { get; set; }
    }
}