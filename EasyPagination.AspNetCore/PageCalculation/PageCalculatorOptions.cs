using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Enums;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class PageCalculatorOptions
    {
        private readonly Dictionary<string, Func<IPaginationParams, Uri, int, int?, PageData>> _pageDataCalculators;
        public PageCalculatorOptions()
        {
            _pageDataCalculators = new Dictionary<string, Func<IPaginationParams, Uri, int, int?, PageData>>();
        }

        public void SetPageCalculation(LinkRelationship relationship,
            Func<IPaginationParams, Uri, int, int?, PageData> function)
            => _pageDataCalculators[relationship.GetLinkHeaderValue()] = function;
        
        public void SetPageCalculation(string relationship,
            Func<IPaginationParams, Uri, int, int?, PageData> function)
            => _pageDataCalculators[relationship] = function;

        public void UseDefaultCalculation(PaginationType paginationType)
        {
            switch (paginationType)
            {
                case PaginationType.Pages:
                    _pageDataCalculators[LinkRelationship.First.GetLinkHeaderValue()] = StaticPagesCalculator.GetFirstPage;
                    _pageDataCalculators[LinkRelationship.Previous.GetLinkHeaderValue()] = StaticPagesCalculator.GetPreviousPage;
                    _pageDataCalculators[LinkRelationship.Next.GetLinkHeaderValue()] = StaticPagesCalculator.GetNextPage;
                    _pageDataCalculators[LinkRelationship.Last.GetLinkHeaderValue()] = StaticPagesCalculator.GetLastPage;
                    break;
                case PaginationType.LimitItems:
                    _pageDataCalculators[LinkRelationship.First.GetLinkHeaderValue()] = StaticLimitOffsetCalculator.GetFirstPage;
                    _pageDataCalculators[LinkRelationship.Previous.GetLinkHeaderValue()] = StaticLimitOffsetCalculator.GetPreviousPage;
                    _pageDataCalculators[LinkRelationship.Next.GetLinkHeaderValue()] = StaticLimitOffsetCalculator.GetNextPage;
                    _pageDataCalculators[LinkRelationship.Last.GetLinkHeaderValue()] = StaticLimitOffsetCalculator.GetLastPage;
                    break;
            }
        }

        internal IPageCalculator GeneratePageCalculator()
        {
            return new DynamicPageCalculator(_pageDataCalculators);
        }
    }
}