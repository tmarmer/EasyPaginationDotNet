using System.Collections.Generic;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public interface IPageCalculator
    {
        IEnumerable<PageData> GetAllPages(PaginationInfo paginationInfo);
    }
}