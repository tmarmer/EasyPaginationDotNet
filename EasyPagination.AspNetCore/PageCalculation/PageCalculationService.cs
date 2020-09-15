using System;
using System.Collections.Generic;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class PageCalculationService : IPageCalculationService
    {
        private readonly Dictionary<Type, IPageCalculator> _pageCalculators;

        public PageCalculationService(Dictionary<Type, IPageCalculator> pageCalculators)
        {
            _pageCalculators = pageCalculators;
        }

        public IEnumerable<PageData> Calculate(PaginationInfo paginationInfo)
        {
            var type = paginationInfo.PaginationParams.GetType();
            
            if (!_pageCalculators.TryGetValue(type, out var calculator))
                throw new InvalidOperationException($"No calculator exists for type '{type.FullName}'");

            return calculator.GetAllPages(paginationInfo);
        }
    }
}