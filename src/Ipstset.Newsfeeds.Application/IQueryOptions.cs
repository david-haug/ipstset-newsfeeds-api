using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application
{
    public interface IQueryOptions
    {
        int Limit { get; set; }
        string StartAfter { get; set; }
        IEnumerable<SortItem> Sort { get; set; }
    }
}
