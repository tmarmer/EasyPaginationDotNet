using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyPagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PaginationSample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumbersController : Controller
    {
        private static readonly List<int> Numbers = Enumerable.Range(0, 1000).ToList();

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<int>), StatusCodes.Status200OK)]
        public IActionResult GetAllNumbers()
        {
            return Ok(Numbers);
        }
        
        [HttpGet("pagination-limit")]
        [ProducesPaginatedResponseType(typeof(int))]
        public IActionResult GetLimitedNumbers([FromQuery] PaginationParams paginationParams)
        {
            if (!IsPaginationValid(paginationParams))
                return BadRequest("Bad pagination");

            var result = paginationParams.Offset < Numbers.Count ? 
                Numbers.GetRange(paginationParams.Offset, paginationParams.Limit)
                : new List<int>();

            return new PaginationObjectResult(result, paginationParams, Numbers.Count);
        }


        private const int MaxLimit = 100;
        
        private bool IsPaginationValid(PaginationParams paginationParams)
            => paginationParams.Offset >= 0 && paginationParams.Limit <= MaxLimit && paginationParams.Limit > 0;
    }
}