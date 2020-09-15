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
    public class FilteredDataController : Controller
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
        public IActionResult GetLimitedData([FromQuery] LimitOffsetParams paginationQuery, [FromQuery] MyFilterParams filterQuery)
        {
            if (!IsPaginationValid(paginationQuery) || !IsFilterValid(filterQuery))
                return BadRequest("Bad pagination");

            var result = paginationQuery.Offset < Data.Count ? 
                Data.Where(x => x.Number >= filterQuery.MinimumNumber && x.Number <= filterQuery.MaximumNumber)
                    .Skip(paginationQuery.Offset)
                    .Take(paginationQuery.Limit)
                    .ToList()
                : new List<DataDto>();

            //Pagination object result must include the items being returned, the initial query options and the max number of items if available
            return new PaginationObjectResult(result, paginationQuery);
        }
        
        //Sample pagination using built-in PagesParams
        [HttpGet("pagination-pages")]
        [ProducesPaginatedResponseType(typeof(DataDto))]
        public IActionResult GetPagedData([FromQuery] PagesParams paginationQuery, [FromQuery] MyFilterParams filterQuery)
        {
            if (!IsPaginationValid(paginationQuery) || !IsFilterValid(filterQuery))
                return BadRequest("Bad pagination");

            var nextRangeStart = paginationQuery.PageNumber * paginationQuery.PageSize;
            var result = paginationQuery.PageNumber < Data.Count ? 
                Data.Where(x => x.Number >= filterQuery.MinimumNumber && x.Number <= filterQuery.MaximumNumber)
                    .Skip(nextRangeStart)
                    .Take(paginationQuery.PageSize)
                    .ToList()
                : new List<DataDto>();

            //Pagination object result must include the items being returned, the initial query options and the max number of items if available
            return new PaginationObjectResult(result, paginationQuery, Data.Count);
        }
        
        //Sample pagination using custom MyPaginationParams
        [HttpGet("my-pagination")]
        [ProducesPaginatedResponseType(typeof(DataDto))]
        public IActionResult GetPagedData([FromQuery] MyPaginationParams paginationQuery)
        {
            if (!IsPaginationValid(paginationQuery))
                return BadRequest("Bad pagination");

            var filteredData = Data.Where(x => x.Number >= paginationQuery.MinimumNumber).ToList();
            
            var result = paginationQuery.Offset < filteredData.Count ? 
                filteredData.GetRange(paginationQuery.Offset, Math.Min(paginationQuery.Limit, filteredData.Count - paginationQuery.Offset))
                : new List<DataDto>();

            //Pagination object result must include the items being returned, the initial query options and the max number of items if available
            return new PaginationObjectResult(result, paginationQuery, filteredData.Count);
        }
        
        private const int MaxLimit = 100;
        
        private bool IsPaginationValid(IPaginationParams limitOffsetPaginationParams)
            => limitOffsetPaginationParams.GetOffset() >= 0 
               && limitOffsetPaginationParams.GetPageSize() <= MaxLimit 
               && limitOffsetPaginationParams.GetPageSize() > 0;

        private bool IsFilterValid(MyFilterParams filterParams)
            => filterParams.MinimumNumber >= 0
               && filterParams.MaximumNumber >= filterParams.MinimumNumber;
    }
}