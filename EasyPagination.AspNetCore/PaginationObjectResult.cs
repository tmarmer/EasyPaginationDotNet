using System.Collections;
using System.Threading.Tasks;
using EasyPagination.AspNetCore.Params;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyPagination.AspNetCore
{
    public class PaginationObjectResult : ObjectResult
    {
        public readonly IPaginationParams PaginationParams;
        public readonly int? TotalItems;
        public readonly int ItemCount;
        
        public PaginationObjectResult(ICollection value, IPaginationParams paginationParams, int? totalItems = null) : base(value)
        {
            PaginationParams = paginationParams;
            TotalItems = totalItems;
            ItemCount = value?.Count ?? 0;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            if (ItemCount == 0)
                response.StatusCode = StatusCodes.Status416RangeNotSatisfiable;
            else if (TotalItems.HasValue && ItemCount == TotalItems)
                response.StatusCode = StatusCodes.Status200OK;
            else
                response.StatusCode = StatusCodes.Status206PartialContent;

            return base.ExecuteResultAsync(context);
        }
    }
}