using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class PageCalculationOptions
    {
        private readonly Dictionary<Type, IPageCalculator> _pageDataCalculators;
        public PageCalculationOptions()
        {
            _pageDataCalculators = new Dictionary<Type, IPageCalculator>();
        }

        public void RegisterPageCalculator<T>(Action<PageCalculatorOptions> options)
            where T : IPaginationParams
        {
            var pageCalculatorOptions = new PageCalculatorOptions();
            options(pageCalculatorOptions);
            _pageDataCalculators[typeof(T)] = pageCalculatorOptions.GeneratePageCalculator();
        }
        
        public void RegisterPageCalculator<T>(IPageCalculator pageCalculator)
            where T : IPaginationParams
        {
            _pageDataCalculators[typeof(T)] = pageCalculator;
        }

        public IPageCalculationService GenerateCalculationService()
        {
            return new PageCalculationService(_pageDataCalculators);
        }
    }
}