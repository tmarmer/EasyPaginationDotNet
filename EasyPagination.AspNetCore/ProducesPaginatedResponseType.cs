using System;

namespace EasyPagination.AspNetCore
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ProducesPaginatedResponseType : Attribute
    {
        public readonly Type PagedItemType;
        public ProducesPaginatedResponseType(Type pagedItemType)
        {
            PagedItemType = pagedItemType;
        }
    }
}