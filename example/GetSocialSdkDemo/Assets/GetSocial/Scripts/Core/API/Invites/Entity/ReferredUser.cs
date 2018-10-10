using System;
using System.Collections.Generic;
#if UNITY_ANDROID
using UnityEngine;
#endif

namespace GetSocialSdk.Core
{
    public sealed class ReferredUser : PublicUser, IConvertableFromNative<ReferredUser>
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

        /// <summary>
        /// Which platform user used to install the app.
        /// </summary>
        /// <value>One of the <see cref="InvitePlatform"/> constants.</value>
        public string InstallPlatform { get; private set; }
        
        /// <summary>
        /// If that is not first install from this device.
        /// </summary>
        /// <value>`false` if that is the first install on this device, `true` otherwise.</value>
        public bool Reinstall { get; private set; }
        
        /// <summary>
        /// If the install was marked as suspicious.
        /// </summary>
        /// <value>`true` if install was marked as suspicious by fraud detection system, `false` otherwise.</value>
        public bool InstallSuspicious { get; private set; }

        public ReferredUser()
        {
            
        }

        internal ReferredUser(Dictionary<string, string> publicProperties, string id, string displayName, string avatarUrl, Dictionary<string, string> identities, DateTime installationDate, string installationChannel) : base(publicProperties, id, displayName, avatarUrl, identities)
        {
            InstallationDate = installationDate;
            InstallationChannel = installationChannel;
        }

        public override string ToString()
        {
            return string.Format("{0}, InstallationDate: {1}, InstallationChannel: {2}, InstallPlatform: {3}, Reinstall: {4}, InstallSuspicious: {5}", base.ToString(), InstallationDate, InstallationChannel, InstallPlatform, Reinstall, InstallSuspicious);
        }

        private bool Equals(ReferredUser other)
        {
            return base.Equals(other) && InstallationDate.Equals(other.InstallationDate) && string.Equals(InstallationChannel, other.InstallationChannel) && string.Equals(InstallPlatform, other.InstallPlatform) && Reinstall == other.Reinstall && InstallSuspicious == other.InstallSuspicious;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ReferredUser && Equals((ReferredUser) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ InstallationDate.GetHashCode();
                hashCode = (hashCode * 397) ^ (InstallationChannel != null ? InstallationChannel.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (InstallPlatform != null ? InstallPlatform.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Reinstall.GetHashCode();
                hashCode = (hashCode * 397) ^ InstallSuspicious.GetHashCode();
                return hashCode;
            }
        }

#if UNITY_ANDROID
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
                InstallPlatform = ajo.CallStr("getInstallPlatform");
                Reinstall = ajo.CallBool("isReinstall");
                InstallSuspicious = ajo.CallBool("isInstallSuspicious");
            }
            return this;
        }
#elif UNITY_IOS
        public new ReferredUser ParseFromJson(Dictionary<string, object> json)
        {
            base.ParseFromJson(json);
            InstallationDate = DateUtils.FromUnixTime((long) json["InstallationDate"]);
            InstallationChannel = (string) json["InstallationChannel"];
            InstallPlatform = (string) json["InstallPlatform"];
            Reinstall = (bool) json["Reinstall"];
            InstallSuspicious = (bool) json["InstallSuspicious"];
            return this;
        }
#endif
    }
}