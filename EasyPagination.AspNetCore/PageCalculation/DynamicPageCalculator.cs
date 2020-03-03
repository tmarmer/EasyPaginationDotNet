using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class DynamicPageCalculator : IPageCalculator
    {
        private readonly Dictionary<string, Func<PaginationInfo, PageData>> _pageDataCalculators;

        public DynamicPageCalculator(Dictionary<string, Func<PaginationInfo, PageData>> pageDataCalculators)
        {
            _pageDataCalculators = pageDataCalculators;
        }

        public IEnumerable<PageData> GetAllPages(PaginationInfo paginationInfo)
        {
            return _pageDataCalculators.Select(pair =>
            {
                var result = pair.Value(paginationInfo);
                result.LinkRelationshipType = pair.Key;
                return result;
            });
        }
    }
}