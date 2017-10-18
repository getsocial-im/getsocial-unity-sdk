using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    public sealed class ReferredUser : PublicUser, IGetSocialBridgeObject<ReferredUser>
    {

        /// <summary>
        /// Date of installation.
        /// </summary>
        /// <value>Date of installation.</value>
        public DateTime InstallationDate { get; private set; }

        public override string ToString()
        {
            return string.Format("[ReferredUser: Id={0}, DisplayName={1}, Identities={2}, InstallationDate={3}]", Id, DisplayName, Identities.ToDebugString(), InstallationDate);
        }

#if UNITY_ANDROID
        
        public UnityEngine.AndroidJavaObject ToAJO()
        {
            throw new System.NotImplementedException("ReferredUser is never passed to Android, only received");
        }

        public ReferredUser ParseFromAJO(UnityEngine.AndroidJavaObject ajo)
        {
            if (ajo.IsJavaNull())
            {
                return null;
            }

            using (ajo)
            {
                base.ParseFromAJO(ajo);                 
                InstallationDate = DateUtils.FromUnixTime(ajo.CallLong("getInstallationDate"));
            }
            return this;
        }
#elif UNITY_IOS
        public string ToJson()
        {
            throw new System.NotImplementedException("ReferredUser is never passed to iOS, only received");
        }

        public ReferredUser ParseFromJson(Dictionary<string, object> json)
        {
            base.ParseFromJson(json);
            InstallationDate = DateUtils.FromUnixTime((long) json["InstallationDate"]);
            return this;
        }
#endif
    }
}