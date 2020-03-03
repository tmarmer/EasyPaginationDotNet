namespace EasyPagination.AspNetCore.Params
{
    public interface IPaginationParams
    {
        int GetPageSize();
        int GetOffset();
        string GetPageSizeName();
        string GetOffsetName();
    }
}