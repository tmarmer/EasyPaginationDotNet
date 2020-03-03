using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class PageCalculationService : IPageCalculationService
    {
        private readonly Dictionary<Type, IPageCalculator> _pageCalculators;

        public PageCalculationService(Dictionary<Type, IPageCalculator> pageCalculators)
        {
            _pageCalculators = pageCalculators;
        }

        public IEnumerable<PageData> Calculate(IPaginationParams paginationParams, Uri baseUri, int itemCount, int? totalItems)
        {
            var type = paginationParams.GetType();
            
            if (!_pageCalculators.TryGetValue(type, out var calculator))
                throw new InvalidOperationException($"No calculator exists for type '{type.FullName}'");

            return calculator.GetAllPages(paginationParams, baseUri, itemCount, totalItems);
        }
    }
}