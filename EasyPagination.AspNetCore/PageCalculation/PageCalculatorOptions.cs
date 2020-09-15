using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Enums;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class PageCalculatorOptions
    {
        private readonly Dictionary<string, Func<PaginationInfo, PageData>> _pageDataCalculators;
        public PageCalculatorOptions()
        {
            _pageDataCalculators = new Dictionary<string, Func<PaginationInfo, PageData>>();
        }

        public void SetPageCalculation(LinkRelationship relationship,
            Func<PaginationInfo, Uri> function)
            => _pageDataCalculators[relationship.GetLinkHeaderValue()] = info => new PageData
            {
                PageLink = function(info),
                LinkRelationshipType = relationship.GetLinkHeaderValue()
            };
        
        public void SetPageCalculation(string relationship,
            Func<PaginationInfo, Uri> function)
            => _pageDataCalculators[relationship] = info => new PageData
            {
                PageLink = function(info),
                LinkRelationshipType = relationship
            };

        public void UseDefaultCalculation(PaginationType paginationType)
        {
            switch (paginationType)
            {
                case PaginationType.Pages:
                    _pageDataCalculators[LinkRelationship.First.GetLinkHeaderValue()] = PagesCalculator.GetFirstPage;
                    _pageDataCalculators[LinkRelationship.Previous.GetLinkHeaderValue()] = PagesCalculator.GetPreviousPage;
                    _pageDataCalculators[LinkRelationship.Next.GetLinkHeaderValue()] = PagesCalculator.GetNextPage;
                    _pageDataCalculators[LinkRelationship.Last.GetLinkHeaderValue()] = PagesCalculator.GetLastPage;
                    break;
                case PaginationType.LimitItems:
                    _pageDataCalculators[LinkRelationship.First.GetLinkHeaderValue()] = LimitOffsetCalculator.GetFirstPage;
                    _pageDataCalculators[LinkRelationship.Previous.GetLinkHeaderValue()] = LimitOffsetCalculator.GetPreviousPage;
                    _pageDataCalculators[LinkRelationship.Next.GetLinkHeaderValue()] = LimitOffsetCalculator.GetNextPage;
                    _pageDataCalculators[LinkRelationship.Last.GetLinkHeaderValue()] = LimitOffsetCalculator.GetLastPage;
                    break;
            }
        }

        internal IPageCalculator GeneratePageCalculator()
        {
            return new DynamicPageCalculator(_pageDataCalculators);
        }
    }
}