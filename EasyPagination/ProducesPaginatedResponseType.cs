using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace EasyPagination
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