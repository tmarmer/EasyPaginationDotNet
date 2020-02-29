using System;
using Microsoft.AspNetCore.Mvc;

namespace EasyPagination.AspNetCore.Params
{
    public abstract class PageNumberPaginationParams : IPaginationParams
    {
        public abstract int PageSize { get; set; }
        public abstract int Offset { get; set; }

        public virtual string RangeType() => "pages";
        public virtual LinkData GetFirstOffset(int actualCount, int? totalItems)
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
        
        public virtual LinkData GetPrevOffset(int actualCount, int? totalItems)
        {
            if (HasFullList(actualCount, totalItems) || Offset == 0)
                return new LinkData { HasLinkHeader = false };
            
            return new LinkData
            {
                Offset = Offset - 1,
                NumberOfItems = PageSize,
                HasLinkHeader = true
            };
        }
        
        public virtual LinkData GetNextOffset(int actualCount, int? totalItems)
        {
            if (IsEndOfList(actualCount, totalItems))
                return new LinkData { HasLinkHeader = false };

            var nextOffset = Offset + 1;
            return new LinkData
            {
                Offset = nextOffset,
                NumberOfItems = Math.Min(totalItems - nextOffset * PageSize ?? PageSize, PageSize),
                HasLinkHeader = true
            };
        }
        
        public virtual LinkData GetLastOffset(int actualCount, int? totalItems)
        {
            if (IsEndOfList(actualCount, totalItems) || !totalItems.HasValue)
                return new LinkData { HasLinkHeader = false };
            
            var numberOfItems = totalItems.Value % PageSize;
            return new LinkData
            {
                Offset = (int) Math.Ceiling(totalItems.Value * 1.0 / PageSize) - 1,
                NumberOfItems = numberOfItems > 0 ? numberOfItems : PageSize,
                HasLinkHeader = true
            };
        }

        protected bool HasFullList(int actualCount, int? totalItems)
        {
            if (!totalItems.HasValue)
                return false;

            return totalItems.Value == actualCount;
        }

        protected bool IsEndOfList(int actualCount, int? totalItems)
        {
            return HasFullList(actualCount, totalItems) || (totalItems.HasValue && (Offset + 1) * PageSize >= totalItems.Value);
        }
    }
}