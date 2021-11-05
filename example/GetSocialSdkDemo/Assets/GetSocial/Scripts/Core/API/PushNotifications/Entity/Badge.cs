using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public sealed class Badge
    {

        [JsonSerializationKey("badge")]
        internal int Value;

        [JsonSerializationKey("increase")]
        internal int Increase;

        internal Badge()
        {
            
        }
        
        /// <summary>
        /// Recipient badge will be increased by value.
        /// </summary>
        /// <param name="value">Value to increase badge by.</param>
        /// <returns>New badge instance.</returns>
        public static Badge IncreaseBy(int value)
        {
            var badge = new Badge();
            badge.Increase = value;
            return badge;
        }
        /// <summary>
        /// Recipient badge will be set to value.
        /// </summary>
        /// <param name="value">Value to be set as badge.></param>
        /// <returns>New badge instance.</returns>
        public static Badge SetTo(int value)
        {
            var badge = new Badge();
            badge.Value = value;
            return badge;
        }
    }
}