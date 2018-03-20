using System;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Collections.Generic;
#endif

namespace GetSocialSdk.Core
{
    public sealed class ReferredUser : PublicUser, IGetSocialBridgeObject<ReferredUser>
    {

        /// <summary>
        /// Date of installation.
        /// </summary>
        /// <value>Date of installation.</value>
        public DateTime InstallationDate { get; private set; }

        
        /// <summary>
        /// One of the channels listed in <see cref="InviteChannelIds"/>.
        /// </summary>
        /// <value>Installation channel.</value>
        public string InstallationChannel { get; private set; }

        public override string ToString()
        {
            return string.Format("[ReferredUser: Id={0}, DisplayName={1}, Identities={2}, InstallationDate={3}, InstallationChannel={4}]", Id, DisplayName, Identities.ToDebugString(), InstallationDate, InstallationChannel);
        }

#if UNITY_ANDROID
        
        public new AndroidJavaObject ToAJO()
        {
            throw new NotImplementedException("ReferredUser is never passed to Android, only received");
        }

        public new ReferredUser ParseFromAJO(AndroidJavaObject ajo)
        {
            if (ajo.IsJavaNull())
            {
                return null;
            }

            using (ajo)
            {
                base.ParseFromAJO(ajo);                 
                InstallationDate = DateUtils.FromUnixTime(ajo.CallLong("getInstallationDate"));
                InstallationChannel = ajo.CallStr("getInstallationChannel");
            }
            return this;
        }
#elif UNITY_IOS
        public new string ToJson()
        {
            throw new NotImplementedException("ReferredUser is never passed to iOS, only received");
        }

        public new ReferredUser ParseFromJson(Dictionary<string, object> json)
        {
            base.ParseFromJson(json);
            InstallationDate = DateUtils.FromUnixTime((long) json["InstallationDate"]);
            InstallationChannel = (string) json["InstallationChannel"];
            return this;
        }
#endif
    }
}