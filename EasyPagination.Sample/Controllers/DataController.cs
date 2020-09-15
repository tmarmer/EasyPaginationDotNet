using System;
using System.Collections.Generic;
using System.Linq;
using EasyPagination.AspNetCore;
using EasyPagination.AspNetCore.Params;
using EasyPagination.Sample.DTOs;
using EasyPagination.Sample.Params;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyPagination.Sample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        private static readonly List<DataDto> Data = Enumerable.Range(0, 1000).Select(x => new DataDto() {Number = x, Id = Guid.NewGuid()}).ToList();

        //Sample endpoint to show all data with no pagination
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<int>), StatusCodes.Status200OK)]
        public IActionResult GetAllNumbers()
        {
            return Ok(Data);
        }
        
        //Sample pagination using built-in LimitOffsetParams
        [HttpGet("pagination-limit")]
        [ProducesPaginatedResponseType(typeof(DataDto))]
        public IActionResult GetLimitedData([FromQuery] LimitOffsetParams paginationQuery)
        {
            if (!IsPaginationValid(paginationQuery))
                return BadRequest("Bad pagination");

            var result = paginationQuery.Offset < Data.Count ? 
                Data.GetRange(paginationQuery.Offset, Math.Min(paginationQuery.Limit, Data.Count - paginationQuery.Offset))
                : new List<DataDto>();

            //Pagination object result must include the items being returned, the initial query options and the max number of items if available
            return new PaginationObjectResult(result, paginationQuery);
        }
        
        //Sample pagination using built-in PagesParams
        [HttpGet("pagination-pages")]
        [ProducesPaginatedResponseType(typeof(DataDto))]
        public IActionResult GetPagedData([FromQuery] PagesParams paginationQuery)
        {
            if (!IsPaginationValid(paginationQuery))
                return BadRequest("Bad pagination");

            var nextRangeStart = paginationQuery.PageNumber * paginationQuery.PageSize;
            var result = paginationQuery.PageNumber < Data.Count ? 
                Data.GetRange(nextRangeStart, Math.Min(paginationQuery.PageSize, Data.Count - nextRangeStart))
                : new List<DataDto>();

            //Pagination object result must include the items being returned, the initial query options and the max number of items if available
            return new PaginationObjectResult(result, paginationQuery, Data.Count);
        }
        
        private const int MaxLimit = 100;
        
        private bool IsPaginationValid(IPaginationParams limitOffsetPaginationParams)
            => limitOffsetPaginationParams.GetOffset() >= 0 && limitOffsetPaginationParams.GetPageSize() <= MaxLimit && limitOffsetPaginationParams.GetPageSize() > 0;
    }
}