using System;
using System.Collections.Generic;
using System.Linq;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class DynamicPageCalculator : IPageCalculator
    {
        private readonly Dictionary<string, Func<IPaginationParams, Uri, int, int?, PageData>> _pageDataCalculators;

        public DynamicPageCalculator(Dictionary<string, Func<IPaginationParams, Uri, int, int?, PageData>> pageDataCalculators)
        {
            _pageDataCalculators = pageDataCalculators;
        }

        public IEnumerable<PageData> GetAllPages(IPaginationParams paginationParams, Uri baseUri, int itemCount, int? totalItems)
        {
            return _pageDataCalculators.Select(pair =>
            {
                var result = pair.Value(paginationParams, baseUri, itemCount, totalItems);
                result.LinkRelationshipType = pair.Key;
                return result;
            });
        }
    }
}