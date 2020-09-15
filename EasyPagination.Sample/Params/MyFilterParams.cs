using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace EasyPagination.Sample.Params
{
    public class MyFilterParams
    {
        [FromQuery(Name = nameof(MinimumNumber))]
        [DefaultValue(0)]
        public int MinimumNumber { get; set; }
        [FromQuery(Name = nameof(MaximumNumber))]
        [DefaultValue(1000)]
        public int MaximumNumber { get; set; }
    }
}