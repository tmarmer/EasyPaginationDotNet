using System;
using System.ComponentModel;
using EasyPagination.AspNetCore.Params;
using Microsoft.AspNetCore.Mvc;

namespace EasyPagination.Sample.Params
{
    public class MyPaginationParams : IPaginationParams
    {
        public const int MaxLimit = 100;
        public const int MinimumLimit = 10;
        public const int DefaultLimit = 25;

        public const string LimitName = "LiMiT";
        public const string OffsetName = "oFfSeT";
        public const string MinimumNumberName = "NiminunMunber";

        private int _limit = DefaultLimit;
        [FromQuery(Name = LimitName)]
        [DefaultValue(DefaultLimit)]
        public int Limit
        {
            get => _limit;
            set => _limit = Math.Min(MaxLimit, Math.Max(MinimumLimit, value));
        }

        private int _offset;
        [FromQuery(Name = OffsetName)]
        public int Offset
        {
            get => _offset;
            set => _offset = Math.Max(0, value);
        }
        
        private int _minimumNumber;
        [FromQuery(Name = MinimumNumberName)]
        public int MinimumNumber
        {
            get => _minimumNumber;
            set => _minimumNumber = Math.Max(0, value);
        }

        public int GetPageSize() => _limit;
        public int GetOffset() => _offset;
        public string GetPageSizeName() => LimitName;
        public string GetOffsetName() => OffsetName;
    }
}