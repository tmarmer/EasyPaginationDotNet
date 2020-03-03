using Microsoft.AspNetCore.Mvc;

namespace EasyPagination.AspNetCore.Params
{
    public class LimitOffsetParams : IPaginationParams
    {
        private const string LimitQueryName = "limit";
        private const string OffsetQueryName = "offset";
        
        [FromQuery(Name = LimitQueryName)] public int Limit { get; set; }
        [FromQuery(Name = OffsetQueryName)] public int Offset { get; set; }
        
        public int GetPageSize() => Limit;
        public int GetOffset() => Offset;
        public string GetPageSizeName() => LimitQueryName;
        public string GetOffsetName() => OffsetQueryName;
    }
}