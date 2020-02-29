namespace EasyPagination.AspNetCore.Params
{
    public interface IPaginationParams
    {
        int PageSize { get; }
        int Offset { get; }
        string RangeType { get; }

        public LinkData GetFirstOffset(int actualCount, int? totalItems);
        public LinkData GetPrevOffset(int actualCount, int? totalItems);
        public LinkData GetNextOffset(int actualCount, int? totalItems);
        public LinkData GetLastOffset(int actualCount, int? totalItems);
    }
}