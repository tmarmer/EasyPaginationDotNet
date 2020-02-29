using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EasyPagination.AspNetCore.Params;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace EasyPagination.AspNetCore
{
    public class PaginationObjectResult : ObjectResult
    {
        private readonly IPaginationParams _paginationParams;
        private readonly int? _fullListCount;
        private readonly int _totalItems;
        
        public PaginationObjectResult(ICollection value, IPaginationParams paginationParams, int? fullListCount) : base(value)
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
            else if (_fullListCount.HasValue && _totalItems == _fullListCount)
                response.StatusCode = StatusCodes.Status200OK;
            else
                response.StatusCode = StatusCodes.Status206PartialContent;
            
            SetPaginationHeaders(context);

            return base.ExecuteResultAsync(context);
        }

        private string GetPageUrl(int pageSize, int offset, Uri baseUri)
        {
            var queryString = HttpUtility.ParseQueryString(baseUri.Query);
            queryString[GetQueryStringName(nameof(IPaginationParams.PageSize))] = pageSize.ToString();
            queryString[GetQueryStringName(nameof(IPaginationParams.Offset))] = offset.ToString();

            if (!string.IsNullOrEmpty(baseUri.Query))
                return baseUri.ToString().Replace(baseUri.Query, $"?{queryString}");

            return $"{baseUri}?{queryString}";
        }

        private string GetQueryStringName(string paramName)
        {
            var fromQueryAttribute = _paginationParams.GetType().GetMember(paramName)[0]
                .GetCustomAttributes(false).OfType<FromQueryAttribute>().FirstOrDefault();

            return fromQueryAttribute?.Name ?? paramName;
        }
        
        private void SetPaginationHeaders(ActionContext context)
        {
            var headers = context.HttpContext.Response.Headers;
            var baseUri = new Uri(context.HttpContext.Request.GetDisplayUrl());
            var startRange = _paginationParams.Offset;
            
            headers[HeaderNames.ContentRange] = $"{startRange}-{startRange + _totalItems - 1}/{_fullListCount}";
            headers[HeaderNames.AcceptRanges] = _paginationParams.RangeType;
            
            var sb = new StringBuilder();
            AddLinkData(sb, _paginationParams.GetFirstOffset(_totalItems, _fullListCount), "first", baseUri);
            AddLinkData(sb, _paginationParams.GetPrevOffset(_totalItems, _fullListCount), "prev", baseUri);
            AddLinkData(sb, _paginationParams.GetNextOffset(_totalItems, _fullListCount), "next", baseUri);
            AddLinkData(sb, _paginationParams.GetLastOffset(_totalItems, _fullListCount), "last", baseUri);
            
            if (sb.Length > 0)
                headers["Link"] = sb.ToString();
        }

        private void AddLinkData(StringBuilder stringBuilder, LinkData linkData, string linkRelationship, Uri baseUri)
        {
            if (!linkData.HasLinkHeader) return;

            if (stringBuilder.Length > 0)
                stringBuilder.Append(',');

            stringBuilder.Append($"<{GetPageUrl(_paginationParams.PageSize, linkData.Offset, baseUri)}>; rel=\"{linkRelationship}\"; items=\"{linkData.NumberOfItems}\"");

        }
    }
}