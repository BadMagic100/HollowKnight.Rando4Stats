using FStats;
using FStats.StatControllers;
using System.Collections.Generic;

namespace RandoStats.Util
{
    public static class TimeByAreaStatWorkaround
    {
        public static List<string> AreaOrder(this TimeByAreaStat t)
        {
            List<string> list = new(AreaName.Areas);
            list.Sort((string a, string b) => t.TimeByArea(b).CompareTo(t.TimeByArea(a)));
            return list;
        }
    }
}
