#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System;

namespace GetSocialSdk.Core
{
    public static class TimeUtils
    {
        
        public static string GetTimeZone(this DateTime timeInTimeZone)
        {
            var diff = timeInTimeZone - DateTime.UtcNow;
            
            var hours = diff.Hours;
            var mins = diff.Minutes;

            // there could be a little difference in mins because of processor time between calls
            var sign = hours >= 0 && mins >= -0.1 ? "+" : "-";

            hours = Math.Abs(hours);
            mins = Math.Abs(mins);
            
            if (mins % 15 != 0)
            {
                var scale = (int) Math.Round(mins / 15.0);
                mins = scale * 15;
            }

            if (mins == 60)
            {
                hours += 1;
                mins = 0;
            }

            var tzStr = "GMT" + sign + hours.ToString("00") + mins.ToString("00");
            
            return tzStr;
        }
    }
}
#endif
