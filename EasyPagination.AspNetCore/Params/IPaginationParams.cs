namespace EasyPagination.AspNetCore.Params
{
    public interface IPaginationParams
    {
        int PageSize { get; }
        int Offset { get; }
        
        string RangeType();
        LinkData GetFirstOffset(int actualCount, int? totalItems);
        LinkData GetPrevOffset(int actualCount, int? totalItems);
        LinkData GetNextOffset(int actualCount, int? totalItems);
        LinkData GetLastOffset(int actualCount, int? totalItems);
    }
}