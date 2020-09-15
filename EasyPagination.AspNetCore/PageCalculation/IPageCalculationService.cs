using System.Collections.Generic;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public interface IPageCalculationService
    {
        IEnumerable<PageData> Calculate(PaginationInfo paginationInfo);
    }
}