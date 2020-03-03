using System;
using System.Linq;
using EasyPagination.AspNetCore.PageCalculation;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace EasyPagination.AspNetCore
{
    public class PaginationActionFilter : IActionFilter
    {
        private readonly IPageCalculationService _pageCalculator;

        public PaginationActionFilter(IPageCalculationService pageCalculator)
        {
            _pageCalculator = pageCalculator;
        }
        
        //No need to do anything prior to execution
        public void OnActionExecuting(ActionExecutingContext context) { }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!(context.Result is PaginationObjectResult paginationResult))
                return;
            
            var headers = context.HttpContext.Response.Headers;
            var baseUri = new Uri(context.HttpContext.Request.GetDisplayUrl());
            var paginationParams = paginationResult.PaginationParams;
            var startRange = paginationParams.GetOffset();
            var itemCount = paginationResult.ItemCount;
            var totalItems = paginationResult.TotalItems;

            var paginationInfo = new PaginationInfo(paginationParams, baseUri, itemCount, totalItems);
            
            headers[HeaderNames.ContentRange] = $"{startRange}-{startRange + itemCount - 1}/{totalItems?.ToString() ?? "*"}";

            var pages = _pageCalculator.Calculate(paginationInfo);
            var linkHeader = string.Join(", ", pages.Where(x => !(x?.PageLink is null)).Select(x => x.ToLinkString()));

            if (!string.IsNullOrEmpty(linkHeader))
                headers["Link"] = linkHeader;
        }
    }
}