using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using EasyPagination.AspNetCore.PageCalculation;
using EasyPagination.AspNetCore.Params;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

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
            //Get necessary variables
            var response = context.HttpContext.Response;
            var paramsOffset = PaginationParams.GetOffset();
            var headers = response.Headers;
            var baseUri = new Uri(context.HttpContext.Request.GetDisplayUrl());
            var pageCalculationService = context.HttpContext.RequestServices.GetService<IPageCalculationService>();
            var paginationInfo = new PaginationInfo(PaginationParams, baseUri, ItemCount, TotalItems);
            
            //Set correct status code
            if (ItemCount == 0 && paramsOffset != 0)
                response.StatusCode = StatusCodes.Status416RangeNotSatisfiable;
            else if (ItemCount == 0 && paramsOffset == 0)
                response.StatusCode = StatusCodes.Status204NoContent;
            else if (TotalItems.HasValue && ItemCount == TotalItems || !TotalItems.HasValue && paramsOffset == 0 && ItemCount < PaginationParams.GetPageSize())
                response.StatusCode = StatusCodes.Status200OK;
            else
                response.StatusCode = StatusCodes.Status206PartialContent;
            
            //Setup headers
            if (ItemCount > 0 || TotalItems.HasValue && TotalItems > 0)
            {
                var contentRangeStart = ItemCount > 0 ? $"{paramsOffset}-{paramsOffset + ItemCount - 1}" : "*";
                headers[HeaderNames.ContentRange] = $"items {contentRangeStart}/{TotalItems?.ToString() ?? "*"}";
            }

            var pages = pageCalculationService.Calculate(paginationInfo);
            var linkHeader = string.Join(", ", pages.Where(x => !(x?.PageLink is null)).Select(x => x.ToLinkString()));

            if (!string.IsNullOrEmpty(linkHeader))
                headers["Link"] = linkHeader;

            //Return response with body if applicable.
            return response.StatusCode == StatusCodes.Status200OK ||
                   response.StatusCode == StatusCodes.Status206PartialContent
                ? new ObjectResult(_result).ExecuteResultAsync(context)
                : Task.CompletedTask;
        }
    }
}