namespace EasyPagination.AspNetCore
{
    public class LinkData
    {
        public int Offset { get; set; }
        public int? NumberOfItems { get; set; }
        public bool HasLinkHeader;
    }
}