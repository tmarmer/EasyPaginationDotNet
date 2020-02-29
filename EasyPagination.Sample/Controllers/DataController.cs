using System;
using System.Collections.Generic;
using System.Linq;
using EasyPagination.AspNetCore;
using EasyPagination.AspNetCore.Params;
using EasyPagination.Sample.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyPagination.Sample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        private static readonly List<DataDto> Data = Enumerable.Range(0, 1000).Select(x => new DataDto() {Number = x, Id = Guid.NewGuid()}).ToList();

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<int>), StatusCodes.Status200OK)]
        public IActionResult GetAllNumbers()
        {
            return Ok(Data);
        }
        
        [HttpGet("pagination-limit")]
        [ProducesPaginatedResponseType(typeof(DataDto))]
        public IActionResult GetLimitedData([FromQuery] LimitPaginationQueryParams paginationQuery)
        {
            if (!IsPaginationValid(paginationQuery))
                return BadRequest("Bad pagination");

            var result = paginationQuery.Offset < Data.Count ? 
                Data.GetRange(paginationQuery.Offset, Math.Min(paginationQuery.PageSize, Data.Count - paginationQuery.Offset))
                : new List<DataDto>();

            return new PaginationObjectResult(result, paginationQuery, Data.Count);
        }
        
        [HttpGet("pagination-pages")]
        [ProducesPaginatedResponseType(typeof(DataDto))]
        public IActionResult GetPagedData([FromQuery] PagedPaginationQueryParams paginationQuery)
        {
            if (!IsPaginationValid(paginationQuery))
                return BadRequest("Bad pagination");

            var nextRangeStart = paginationQuery.Offset * paginationQuery.PageSize;
            var result = paginationQuery.Offset < Data.Count ? 
                Data.GetRange(nextRangeStart, Math.Min(paginationQuery.PageSize, Data.Count - nextRangeStart))
                : new List<DataDto>();

            return new PaginationObjectResult(result, paginationQuery, Data.Count);
        }
        
        private const int MaxLimit = 100;
        
        private bool IsPaginationValid(IPaginationParams limitOffsetPaginationParams)
            => limitOffsetPaginationParams.Offset >= 0 && limitOffsetPaginationParams.PageSize <= MaxLimit && limitOffsetPaginationParams.PageSize > 0;
    }
}