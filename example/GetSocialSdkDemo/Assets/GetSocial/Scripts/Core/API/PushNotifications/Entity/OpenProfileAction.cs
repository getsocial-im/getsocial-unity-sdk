using System;

namespace GetSocialSdk.Core
{
    public sealed class OpenProfileAction : NotificationAction
    {
        public OpenProfileAction(string userId)
        {
            UserId = userId;
        }

        public override ActionType Type {
            get
            {
                return ActionType.OpenProfile;
            }
        }

        public string UserId { get; private set; }
    }
}