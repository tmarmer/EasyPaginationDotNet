using System;
using System.Collections.Generic;
using EasyPagination.AspNetCore.Enums;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public class LimitOffsetCalculator : IPageCalculator
    {
        public IEnumerable<PageData> GetAllPages(PaginationInfo paginationInfo) =>
            new[]
            {
                GetFirstPage(paginationInfo),
                GetPreviousPage(paginationInfo),
                GetNextPage(paginationInfo),
                GetLastPage(paginationInfo)
            };
        
        public static PageData GetFirstPage(PaginationInfo paginationInfo)
        {
            var totalItems = paginationInfo.TotalItems;
            var itemCount = paginationInfo.ItemCount;
            var paginationParams = paginationInfo.PaginationParams;
            var baseUri = paginationInfo.BaseUri;

            if (HasFullList(itemCount, totalItems) || paginationParams.GetOffset() == 0)
                return new PageData();
            
            return new PageData()
            {
                PageLink = paginationParams.GenerateLinkHeaderUri(baseUri, 0),
                LinkRelationshipType = LinkRelationship.First.GetLinkHeaderValue()
            };
        }
        
        public static PageData GetPreviousPage(PaginationInfo paginationInfo)
        {
            var totalItems = paginationInfo.TotalItems;
            var itemCount = paginationInfo.ItemCount;
            var paginationParams = paginationInfo.PaginationParams;
            var baseUri = paginationInfo.BaseUri;
            
            var offset = paginationParams.GetOffset();
            var pageSize = paginationParams.GetPageSize();
            
            if (HasFullList(itemCount, totalItems) || offset == 0)
                return new PageData();
            
            return new PageData()
            {
                PageLink = paginationParams.GenerateLinkHeaderUri(baseUri, Math.Max(0, offset - pageSize)),
                LinkRelationshipType = LinkRelationship.Previous.GetLinkHeaderValue()
            };
        }
        
        public static PageData GetNextPage(PaginationInfo paginationInfo)
        {
            var totalItems = paginationInfo.TotalItems;
            var itemCount = paginationInfo.ItemCount;
            var paginationParams = paginationInfo.PaginationParams;
            var baseUri = paginationInfo.BaseUri;
            
            if (IsEndOfList(paginationParams, itemCount, totalItems))
                return new PageData();
            
            var offset = paginationParams.GetOffset();
            var pageSize = paginationParams.GetPageSize();
            
            return new PageData()
            {
                PageLink = paginationParams.GenerateLinkHeaderUri(baseUri, pageSize + offset),
                LinkRelationshipType = LinkRelationship.Next.GetLinkHeaderValue()
            };
        }
        
        public static PageData GetLastPage(PaginationInfo paginationInfo)
        {
            var totalItems = paginationInfo.TotalItems;
            var itemCount = paginationInfo.ItemCount;
            var paginationParams = paginationInfo.PaginationParams;
            var baseUri = paginationInfo.BaseUri;
            
            if (!totalItems.HasValue || IsEndOfList(paginationParams, itemCount, totalItems))
                return new PageData();
            
            var offset = paginationParams.GetOffset();
            var pageSize = paginationParams.GetPageSize();
            
            var numberOfItems = ((int) Math.Ceiling((totalItems.Value - offset) * 1.0 / pageSize) - 1) * pageSize + offset;
            return new PageData()
            {
                PageLink = paginationParams.GenerateLinkHeaderUri(baseUri, numberOfItems),
                LinkRelationshipType = LinkRelationship.Last.GetLinkHeaderValue()
            };
        }

        private static bool HasFullList(int itemCount, int? totalItems)
        {
            if (!totalItems.HasValue)
                return false;

            return totalItems.Value == itemCount;
        }

        private static bool IsEndOfList(IPaginationParams paginationParams, int itemCount, int? totalItems) 
            => HasFullList(itemCount, totalItems) || (totalItems.HasValue && paginationParams.GetOffset() + paginationParams.GetPageSize() >= totalItems);
    }
}