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
        private static readonly List<DataDto> Numbers = Enumerable.Range(0, 1000).Select(x => new DataDto() {Number = x, Id = Guid.NewGuid()}).ToList();

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<int>), StatusCodes.Status200OK)]
        public IActionResult GetAllNumbers()
        {
            return Ok(Numbers);
        }
        
        [HttpGet("pagination-limit")]
        [ProducesPaginatedResponseType(typeof(DataDto))]
        public IActionResult GetLimitedNumbers([FromQuery] LimitOffsetPaginationParams limitOffsetPaginationParams)
        {
            if (!IsPaginationValid(limitOffsetPaginationParams))
                return BadRequest("Bad pagination");

            var result = limitOffsetPaginationParams.Offset < Numbers.Count ? 
                Numbers.GetRange(limitOffsetPaginationParams.Offset, limitOffsetPaginationParams.PageSize)
                : new List<DataDto>();

            return new PaginationObjectResult(result, limitOffsetPaginationParams, Numbers.Count);
        }
        
        [HttpGet("pagination-pages")]
        [ProducesPaginatedResponseType(typeof(DataDto))]
        public IActionResult GetPagedNumbers([FromQuery] PageNumberPaginationParams pageNumberPaginationParams)
        {
            if (!IsPaginationValid(pageNumberPaginationParams))
                return BadRequest("Bad pagination");

            var result = pageNumberPaginationParams.Offset < Numbers.Count ? 
                Numbers.GetRange(pageNumberPaginationParams.Offset * pageNumberPaginationParams.PageSize, pageNumberPaginationParams.PageSize)
                : new List<DataDto>();

            return new PaginationObjectResult(result, pageNumberPaginationParams, Numbers.Count);
        }
        
        private const int MaxLimit = 100;
        
        private bool IsPaginationValid(IPaginationParams limitOffsetPaginationParams)
            => limitOffsetPaginationParams.Offset >= 0 && limitOffsetPaginationParams.PageSize <= MaxLimit && limitOffsetPaginationParams.PageSize > 0;
    }
}