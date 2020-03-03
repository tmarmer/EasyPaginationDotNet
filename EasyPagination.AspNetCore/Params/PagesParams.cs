using Microsoft.AspNetCore.Mvc;

namespace EasyPagination.AspNetCore.Params
{
    public class PagesParams : IPaginationParams
    {
        private const string PageSizeQueryName = "pageSize";
        private const string PageNumberQueryName = "page";
        
        [FromQuery(Name = PageSizeQueryName)] public int PageSize { get; set; }
        [FromQuery(Name = PageNumberQueryName)] public int PageNumber { get; set; }
        
        public int GetPageSize() => PageSize;
        public int GetOffset() => PageNumber;
        public string GetPageSizeName() => PageSizeQueryName;
        public string GetOffsetName() => PageNumberQueryName;
    }
}