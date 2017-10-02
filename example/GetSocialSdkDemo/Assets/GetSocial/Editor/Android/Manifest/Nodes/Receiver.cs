using System.Collections.Generic;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public class Receiver : AndroidManifestNode
    {
        public Receiver(string name, IntentFilter intentFilter) : base(
            "receiver", 
            ApplicationTag, 
            new Dictionary<string, string>{{NameAttribute, name}})
        {
            AddChild(intentFilter);
        }

        public override string ToString()
        {
            return string.Format("Receiver {0}", Attributes[NameAttribute]);
        }
    }
}