using System;

namespace GetSocialSdk.Core
{
    public sealed class OpenActivityAction : NotificationAction
    {
        public OpenActivityAction(string activityId)
        {
            ActivityId = activityId;
        }

        public override ActionType Type {
            get
            {
                return ActionType.OpenActivity;
            }
        }

        public string ActivityId { get; private set; }
    }
}