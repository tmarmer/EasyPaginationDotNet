using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace EasyPagination
{
    public class PaginationObjectResult : ObjectResult
    {
        private readonly PaginationParams _paginationParams;
        private readonly int _fullListCount;
        private readonly int _totalItems;
        
        public PaginationObjectResult(ICollection value, PaginationParams paginationParams, int fullListCount) : base(value)
        {
            _paginationParams = paginationParams;
            _fullListCount = fullListCount;
            _totalItems = value?.Count ?? 0;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            if (_totalItems == 0)
                response.StatusCode = StatusCodes.Status416RangeNotSatisfiable;
            else if (_totalItems == _fullListCount)
                response.StatusCode = StatusCodes.Status200OK;
            else
                response.StatusCode = StatusCodes.Status206PartialContent;
            
            SetPaginationHeaders(context);

            return base.ExecuteResultAsync(context);
        }
            
        private string GetPageUrl(int limit, int offset, Uri baseUri)
        {
            var queryString = HttpUtility.ParseQueryString(baseUri.Query);
            queryString[nameof(PaginationParams.Limit).ToLower()] = limit.ToString();
            queryString[nameof(PaginationParams.Offset).ToLower()] = offset.ToString();

            if (!string.IsNullOrEmpty(baseUri.Query))
                return baseUri.ToString().Replace(baseUri.Query, $"?{queryString}");

            return $"{baseUri}?{queryString}";
        }
        
        private void SetPaginationHeaders(ActionContext context)
        {
            var headers = context.HttpContext.Response.Headers;
            var baseUri = new Uri(context.HttpContext.Request.GetDisplayUrl());
            var pageLimit = _paginationParams.Limit;
            var startRange = _paginationParams.Offset;
            
            headers[HeaderNames.ContentRange] = $"{startRange}-{startRange + _totalItems - 1}/{_fullListCount}";
            headers[HeaderNames.AcceptRanges] = "items";
            
            var sb = new StringBuilder();
            var nextPageOffset = startRange + pageLimit;
            
            // Has prev page
            if (_paginationParams.Offset > 0)
            {
                var maxPageSize = Math.Min(pageLimit, _fullListCount);
                //first page url
                sb.Append($"<{GetPageUrl(pageLimit, 0, baseUri)}>; rel=\"first\"; items=\"{maxPageSize}\"");
                
                sb.Append(',');
                
                //prev page url
                sb.Append($"<{GetPageUrl(pageLimit, Math.Max(startRange - pageLimit, 0), baseUri)}>; rel=\"prev\"; items=\"{maxPageSize}\"");
            }
            
            // Has next page
            if (nextPageOffset < _fullListCount)
            {
                //Next page url
                sb.Append(
                    $"<{GetPageUrl(pageLimit, nextPageOffset, baseUri)}>; rel=\"next\"; items=\"{Math.Min(pageLimit, _fullListCount - nextPageOffset)}\"");
                
                sb.Append(',');
                
                //Last page url
                sb.Append(
                    $"<{GetPageUrl(pageLimit, _fullListCount - pageLimit, baseUri)}>; rel=\"last\"; items=\"{pageLimit}\"");
            }

            if (sb.Length > 0)
                headers["Link"] = sb.ToString();
        }
    }
}