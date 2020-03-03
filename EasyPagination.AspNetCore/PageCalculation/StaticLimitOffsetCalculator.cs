using System;
using EasyPagination.AspNetCore.Enums;
using EasyPagination.AspNetCore.Params;

namespace EasyPagination.AspNetCore.PageCalculation
{
    public static class StaticLimitOffsetCalculator
    {
        public static PageData GetFirstPage(IPaginationParams paginationParams, Uri baseUri, int itemCount, int? totalItems)
        {
            if (HasFullList(itemCount, totalItems) || paginationParams.GetOffset() == 0)
                return new PageData();
            
            return new PageData()
            {
                PageLink = paginationParams.GenerateLinkHeaderUri(baseUri, 0),
                LinkRelationshipType = LinkRelationship.First.GetLinkHeaderValue()
            };
        }
        
        public static PageData GetPreviousPage(IPaginationParams paginationParams, Uri baseUri, int itemCount, int? totalItems)
        {
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
        
        public static PageData GetNextPage(IPaginationParams paginationParams, Uri baseUri, int itemCount, int? totalItems)
        {
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
        
        public static PageData GetLastPage(IPaginationParams paginationParams, Uri baseUri, int itemCount, int? totalItems)
        {
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
        {
            return HasFullList(itemCount, totalItems) || (totalItems.HasValue && paginationParams.GetOffset() + paginationParams.GetPageSize() >= totalItems);
        }
    }
}