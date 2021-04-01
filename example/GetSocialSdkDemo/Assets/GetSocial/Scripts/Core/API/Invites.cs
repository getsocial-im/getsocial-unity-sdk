using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    public static class Invites
    {
        /// <summary>
        /// Get the list of available invite channels. Contains only channels enabled on the Dashboard and available on the device.
        /// </summary>
        /// <param name="success">Called with a list of channels</param>
        /// <param name="failure">Called with error if operation fails</param>
        public static void GetAvailableChannels(Action<List<InviteChannel>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetAvailableChannels(success, failure);
        }

        /// <summary>
        /// Invite friends via a specified invite channel.
        /// </summary>
        /// <param name="customInviteContent">Custom content to override the default content provided from the Dashboard.</param>
        /// <param name="channelId">The channel through which the invite will be sent, one of the constants defined in <see cref="InviteChannelIds"/>.</param>
        /// <param name="success">Called when the invite process is complete.</param>
        /// <param name="cancel">Called if a user canceled an invite.</param>
        /// <param name="failure">Called with error if operation fails</param>
        public static void Send(InviteContent customInviteContent, string channelId, Action success, Action cancel, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.Send(customInviteContent, channelId, success, cancel, failure);
        }

        /// <summary>
        /// Create invite with localized values and a link.
        /// </summary>
        /// <param name="customInviteContent">Custom content to override the default content provided from the Dashboard.</param>
        /// <param name="success">Called with invite content when it is created.</param>
        /// <param name="failure">Called with error if operation fails</param>
        public static void Create(InviteContent customInviteContent, Action<Invite> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.Create(customInviteContent, success, failure);
        }

        /// <summary>
        /// Creates a Smart Link with user referral data attached used for Smart Invites. 
        /// </summary>
        /// <param name="linkParams">Link customization parameters. More info @see <a href="https://docs.getsocial.im/guides/smart-links/parameters/">here</a></param>
        /// <param name="success">Called when the url creation is finished.</param>
        /// <param name="failure">Called with error if operation fails</param>
        public static void CreateLink(Dictionary<string, object> linkParams, Action<string> success,
            Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.CreateLink(linkParams, success, failure);
        }

        /// <summary>
        /// Returns list of users who were referred by this user for a specific event.
        /// </summary>
        /// <param name="query">Users query.</param>
        /// <param name="success">Called with list of referred users. If there is no referred user, the list is empty.</param>
        /// <param name="failure">Called with error if operation fails</param>
        public static void GetReferredUsers(PagingQuery<ReferralUsersQuery> query, Action<PagingResult<ReferralUser>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetReferredUsers(query, success, failure);
        }

        /// <summary>
        /// Returns list of users who are referrers for this user for a specific event.
        /// </summary>
        /// <param name="query">Users query.</param>
        /// <param name="success">Called with list of referred users. If there is no referred user, the list is empty.</param>
        /// <param name="failure">Called with error if operation fails</param>
        public static void GetReferrerUsers(PagingQuery<ReferralUsersQuery> query, Action<PagingResult<ReferralUser>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetReferrerUsers(query, success, failure);
        }

        /// <summary>
        /// Sets referrer id for current user. 
        /// </summary>
        /// <param name="userId">Id of referrer user.</param>
        /// <param name="eventName">Referrer event.</param>
        /// <param name="customData">Custom key-value pairs.</param>
        /// <param name="success">Called when the referrer setting is finished.</param>
        /// <param name="failure">Called with error if operation fails</param>
        public static void SetReferrer(UserId userId, string eventName, Dictionary<string, string> customData, Action success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.SetReferrer(userId, eventName, customData, success, failure);
        }

        /// <summary>
        /// Set listener to receive referral data if there is any.
        /// </summary>
        /// <param name="action">to be called with a new referral data</param>
        public static void SetOnReferralDataReceivedListener(Action<ReferralData> action)
        {
            GetSocialFactory.Bridge.SetOnReferralDataReceivedListener(action);
        }
    }
}
