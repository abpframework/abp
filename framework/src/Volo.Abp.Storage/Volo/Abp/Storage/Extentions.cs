using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Storage
{
    public static class Extentions
    {
        public static long ToUnixTimeSeconds(this DateTimeOffset dateTimeOffset)
        {
            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var unixTimeStampInTicks = (dateTimeOffset.ToUniversalTime() - unixStart).Ticks;
            return unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }

        public static List<T2> SelectToListOrEmpty<T1, T2>(this IEnumerable<T1> e, Func<T1, T2> f)
        {
            return e == null ? new List<T2>() : e.Select(f).ToList();
        }

        public static List<T1> WhereToListOrEmpty<T1>(this IEnumerable<T1> e, Func<T1, bool> f)
        {
            return e == null ? new List<T1>() : e.Where(f).ToList();
        }
    }
}