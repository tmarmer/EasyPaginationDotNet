using System;

namespace EasyPagination.AspNetCore
{
    public class PageData
    {
        public Uri PageLink { get; set; }
        public string LinkRelationshipType { get; set; }

        public string ToLinkString()
            => $"<{PageLink}>; rel=\"{LinkRelationshipType}\"";
            
    }
}