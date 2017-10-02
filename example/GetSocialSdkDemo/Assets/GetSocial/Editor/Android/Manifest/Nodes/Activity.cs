using System.Collections.Generic;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public class Activity : AndroidManifestNode
    {
        public Activity(string name, IntentFilter intentFilter) : base(
            "activity", 
            ApplicationTag, 
            new Dictionary<string, string>{{NameAttribute, name}})
        {
            AddChild(intentFilter);
        }

        public override string ToString()
        {
            return string.Format("Activity {0}", Attributes[NameAttribute]);
        }
    }
}