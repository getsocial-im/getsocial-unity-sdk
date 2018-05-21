using System;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Collections.Generic;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// The author of <see cref="ActivityPost"/>.
    /// </summary>
    public sealed class PostAuthor : PublicUser, IConvertableFromNative<PostAuthor>
    {
        /// <summary>
        /// Gets a value indicating whether this user is verified.
        /// </summary>
        /// <value><c>true</c> if this user is verified; otherwise, <c>false</c>.</value>
        public bool IsVerified { get; private set; }

        public override string ToString()
        {
            return string.Format("[PostAuthor: Id={0}, DisplayName={1}, Identities={2}, IsVerified={3}]", Id, DisplayName, Identities.ToDebugString(), IsVerified);
        }


#if UNITY_ANDROID
        public new PostAuthor ParseFromAJO(AndroidJavaObject ajo)
        {
            using (ajo)
            {
                base.ParseFromAJO(ajo);
                IsVerified = ajo.CallBool("isVerified");
            }
            return this;
        }
#elif UNITY_IOS
        public new PostAuthor ParseFromJson(Dictionary<string, object> jsonDic)
        {
            base.ParseFromJson(jsonDic);
            IsVerified = (bool) jsonDic["IsVerified"];
            return this;
        }

#endif
    }
}