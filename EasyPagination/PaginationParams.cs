using System.ComponentModel;

namespace EasyPagination
{
    public class PaginationParams
    {
        private const int DefaultLimit = 25;
        private const int DefaultOffset = 0;
        
        [DefaultValue(DefaultLimit)] public int Limit { get; set; } = DefaultLimit;
        [DefaultValue(DefaultOffset)] public int Offset { get; set; } = DefaultOffset;
    }
}