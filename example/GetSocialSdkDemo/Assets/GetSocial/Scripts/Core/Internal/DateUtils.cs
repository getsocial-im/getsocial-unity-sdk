using System;

namespace GetSocialSdk.Core
{
    public static class DateUtils
    {
        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long) (dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}