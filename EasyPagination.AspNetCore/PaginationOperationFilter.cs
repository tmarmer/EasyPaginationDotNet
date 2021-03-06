using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EasyPagination.AspNetCore
{
    public class PaginationOperationFilter : IOperationFilter
    {
        private const string RangeNotSatisfiableDescription = "Pagination parameters out of range.";
        private const string OkDescription = "List of all items. No other pages.";
        private const string PartialContentDescription = "List of some items. Other pages will be linked in the 'Links' header.";
        private const string NoContentDescription = "No results available, but no offset so input is valid.";
        
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attr = context.MethodInfo.GetCustomAttributes(false).OfType<ProducesPaginatedResponseType>().FirstOrDefault();
            if (attr is null) return;
            
            var enumerablePagedItem = typeof(IEnumerable<>).MakeGenericType(attr.PagedItemType);
            var schema = context.SchemaGenerator.GenerateSchema(enumerablePagedItem, context.SchemaRepository);
            var responses = operation.Responses;

            EnsureApiResponse(responses, StatusCodes.Status416RangeNotSatisfiable, RangeNotSatisfiableDescription);
            EnsureApiResponse(responses, StatusCodes.Status204NoContent, NoContentDescription);
            UpdateResponse(EnsureApiResponse(responses, StatusCodes.Status200OK, OkDescription), schema);
            UpdateResponse(EnsureApiResponse(responses, StatusCodes.Status206PartialContent, PartialContentDescription), schema);
        }

        private void UpdateResponse(OpenApiResponse response, OpenApiSchema schema)
        {
            if (response.Content.Count == 0)
            {
                response.Content[MediaTypeNames.Application.Json] = new OpenApiMediaType();
            }

            foreach (var mediaType in response.Content.Values)
            {
                mediaType.Schema = schema;
            }
        }

        private OpenApiResponse EnsureApiResponse(OpenApiResponses apiResponses, int responseCode, string defaultDescription)
        {
            var responseCodeAsString = responseCode.ToString();
            if (!apiResponses.TryGetValue(responseCodeAsString, out var response))
            {
                response = new OpenApiResponse()
                {
                    Description = defaultDescription
                };

                apiResponses[responseCodeAsString] = response;
            }
            
            //Replace swashbuckle default 200 description with our default.
            if (responseCode == 200 && response.Description == "Success")
                response.Description = defaultDescription;

            return response;
        }
    }
}