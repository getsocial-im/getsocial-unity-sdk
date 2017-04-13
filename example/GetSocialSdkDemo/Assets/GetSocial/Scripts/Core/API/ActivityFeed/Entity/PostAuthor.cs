using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// User that is received in the case of conflict when adding auth identity.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public sealed class PostAuthor : PublicUser, IGetSocialBridgeObject<PostAuthor>
    {
        /// <summary>
        /// Gets a value indicating whether this user is verified.
        /// </summary>
        /// <value><c>true</c> if this user is verified; otherwise, <c>false</c>.</value>
        public bool IsVerified { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}, IsVerified: {1}", base.ToString(), IsVerified);
        }

#if UNITY_ANDROID
        public new UnityEngine.AndroidJavaObject ToAJO()
        {
            throw new System.NotImplementedException("PostAuthor is never passed to Android");
        }

        public new PostAuthor ParseFromAJO(UnityEngine.AndroidJavaObject ajo)
        {
            using (ajo)
            {
                base.ParseFromAJO(ajo);
                IsVerified = ajo.CallBool("isVerified");
            }
            return this;
        }
#elif UNITY_IOS

        public new string ToJson()
        {
            throw new System.NotImplementedException("PostAuthor is never passed to iOS");
        }

        public new PostAuthor ParseFromJson(string json)
        {
            return ParseFromJson(json.ToDict());
        }

        public new PostAuthor ParseFromJson(Dictionary<string, object> jsonDic)
        {
            base.ParseFromJson(jsonDic);
            IsVerified = (bool) jsonDic[IsVerifiedFieldName];
            return this;
        }

        static string IsVerifiedFieldName
        {
            get { return ReflectionUtils.GetMemberName((PostAuthor c) => c.IsVerified); }
        }

#endif
    }
}