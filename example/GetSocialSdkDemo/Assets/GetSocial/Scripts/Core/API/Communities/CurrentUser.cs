using System;

namespace GetSocialSdk.Core
{
    public class CurrentUser : PrivateUser
    {
        /// <summary>
        /// Requests a bulk change of properties for the current user.
        /// </summary>
        /// <param name="userUpdate">New user details.</param>
        /// <param name="callback">A callback to indicate if this operation was successful.</param>
        /// <param name="failure">Called if operation failed.</param>
        public void UpdateDetails(UserUpdate userUpdate, Action callback, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.UpdateDetails(userUpdate, callback, failure);
        }

        /// <summary>
        /// Adds Identity for the specified provider.
        /// </summary>
        /// <param name="identity">Identity to be added.</param>
        /// <param name="success">A callback to indicate if this operation was successful.</param>
        /// <param name="conflict">Called if there was a conflict.</param>
        /// <param name="failure">Called if operation failed.</param>
        public void AddIdentity(Identity identity, Action success, Action<ConflictUser> conflict,
            Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.AddIdentity(identity, success, conflict, failure);
        }

        /// <summary>
        /// Removes Identity for the specified provider.
        /// </summary>
        /// <param name="providerId">The provider connected to an auth identity on the current user to remove.</param>
        /// <param name="callback">A callback to indicate if this operation was successful.</param>
        /// <param name="failure">Called if operation failed.</param>
        public void RemoveIdentity(string providerId, Action callback, Action<GetSocialError> failure) 
        {
            GetSocialFactory.Bridge.RemoveIdentity(providerId, callback, failure);
        }

        /// <summary>
        /// Refresh properties of current user.
        /// </summary>
        /// <param name="callback">A callback to indicate if this operation was successful.</param>
        /// <param name="failure">Called if operation failed.</param>
        public void Refresh(Action success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.Refresh(success, failure);
        }

        public override string ToString()
        {
            return $"{base.ToString()}";
        }

    }
}