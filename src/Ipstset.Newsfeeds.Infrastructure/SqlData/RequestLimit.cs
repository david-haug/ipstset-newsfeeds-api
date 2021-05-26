using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Infrastructure.SqlData
{
    public class RequestLimit
    {
        public static int Max => 100;
        public static int Get(int limitRequested)
        {
            return limitRequested == 0 || limitRequested > Max ? Max : limitRequested;
        }
    }
}
