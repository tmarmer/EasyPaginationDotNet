using System.Collections;
using System.Threading.Tasks;
using EasyPagination.AspNetCore.Params;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyPagination.AspNetCore
{
    public class PaginationObjectResult : IActionResult
    {
        private readonly ICollection _result;
        
        public readonly IPaginationParams PaginationParams;
        public readonly int? TotalItems;
        public readonly int ItemCount;

        public PaginationObjectResult(ICollection value, IPaginationParams paginationParams, int? totalItems = null)
        {
            PaginationParams = paginationParams;
            TotalItems = totalItems;
            ItemCount = value?.Count ?? 0;
            _result = value;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            var paramsOffset = PaginationParams.GetOffset();

            if (ItemCount == 0 && paramsOffset != 0)
                response.StatusCode = StatusCodes.Status416RangeNotSatisfiable;
            else if (ItemCount == 0 && paramsOffset == 0)
                response.StatusCode = StatusCodes.Status204NoContent;
            else if (TotalItems.HasValue && ItemCount == TotalItems)
                response.StatusCode = StatusCodes.Status200OK;
            else
                response.StatusCode = StatusCodes.Status206PartialContent;

            return response.StatusCode == StatusCodes.Status200OK ||
                   response.StatusCode == StatusCodes.Status206PartialContent
                ? new ObjectResult(_result).ExecuteResultAsync(context)
                : Task.CompletedTask;
        }
    }
}