using System;
using Microsoft.AspNetCore.Mvc;

namespace EasyPagination.AspNetCore.Params
{
    public class LimitOffsetPaginationParams : IPaginationParams
    {
        [FromQuery(Name = "limit")] public int PageSize { get; set; }
        [FromQuery(Name = "offset")] public int Offset { get; set; }

        public string RangeType() => "items";
        public LinkData GetFirstOffset(int actualCount, int? totalItems)
        {
            if (HasFullList(actualCount, totalItems) || Offset == 0)
                return new LinkData { HasLinkHeader = false };
            
            return new LinkData
            {
                Offset = 0,
                NumberOfItems = PageSize,
                HasLinkHeader = true
            };
        }
        
        public LinkData GetPrevOffset(int actualCount, int? totalItems)
        {
            if (HasFullList(actualCount, totalItems) || Offset == 0)
                return new LinkData { HasLinkHeader = false };
            
            return new LinkData
            {
                Offset = Math.Max(0, Offset - PageSize),
                NumberOfItems = PageSize,
                HasLinkHeader = true
            };
        }
        
        public LinkData GetNextOffset(int actualCount, int? totalItems)
        {
            if (IsEndOfList(actualCount, totalItems))
                return new LinkData { HasLinkHeader = false };

            var nextOffset = PageSize + Offset;
            return new LinkData
            {
                Offset = nextOffset,
                NumberOfItems = Math.Min(totalItems - nextOffset ?? PageSize, PageSize),
                HasLinkHeader = true
            };
        }
        
        public LinkData GetLastOffset(int actualCount, int? totalItems)
        {
            if (IsEndOfList(actualCount, totalItems) || !totalItems.HasValue)
                return new LinkData { HasLinkHeader = false };

            var numberOfItems = totalItems.Value % PageSize;
            return new LinkData
            {
                Offset = ((int) Math.Ceiling((totalItems.Value - Offset) * 1.0 / PageSize) - 1) * PageSize + Offset,
                NumberOfItems =  numberOfItems > 0 ? numberOfItems : PageSize,
                HasLinkHeader = true
            };
        }

        private bool HasFullList(int actualCount, int? totalItems)
        {
            if (!totalItems.HasValue)
                return false;

            return totalItems.Value == actualCount;
        }

        private bool IsEndOfList(int actualCount, int? totalItems)
        {
            return HasFullList(actualCount, totalItems) || (totalItems.HasValue && Offset + PageSize >= totalItems);
        }
    }
}