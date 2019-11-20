#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal static class ThriftConversions
    {
        public static Dictionary<string, string> IdentitiesToDictionary(this List<THIdentity> identities)
        {
            identities = identities ?? new List<THIdentity>();
            return identities.ToDictionary(identity => identity.Provider, identity => identity.ProviderId);
        }

        public static ConflictUser ToConflictUser(this THPrivateUser conflictUser)
        {
            return new ConflictUser(
                publicProperties: conflictUser.PublicProperties, 
                id: conflictUser.Id, 
                displayName:conflictUser.DisplayName, 
                avatarUrl:conflictUser.AvatarUrl, 
                identities:IdentitiesToDictionary(conflictUser.Identities)
                );
        }

        public static SuggestedFriend ToSuggestedFriend(this THSuggestedFriend suggestedFriend)
        {
            return new SuggestedFriend(
                publicProperties: suggestedFriend.User.PublicProperties, 
                id: suggestedFriend.User.Id, 
                displayName:suggestedFriend.User.DisplayName, 
                avatarUrl:suggestedFriend.User.AvatarUrl, 
                identities:IdentitiesToDictionary(suggestedFriend.User.Identities),
                mutualFriendsCount: suggestedFriend.MutualFriends
                );
        }
        
        public static PublicUser ToPublicUser(this THPublicUser publicUser)
        {
            return new PublicUser(
                publicProperties: publicUser.PublicProperties, 
                id: publicUser.Id, 
                displayName:publicUser.DisplayName, 
                avatarUrl:publicUser.AvatarUrl, 
                identities:IdentitiesToDictionary(publicUser.Identities)
                );
        }

        public static PublicUser ToPublicUser(this THPostAuthor publicUser)
        {
            return new PublicUser(
                publicProperties: publicUser.PublicProperties, 
                id: publicUser.Id, 
                displayName:publicUser.DisplayName, 
                avatarUrl:publicUser.AvatarUrl, 
                identities:IdentitiesToDictionary(publicUser.Identities)
                );
        }

        public static PostAuthor ToPostAuthor(this THPostAuthor publicUser)
        {
            return new PostAuthor(
                publicProperties: publicUser.PublicProperties, 
                id: publicUser.Id, 
                displayName:publicUser.DisplayName, 
                avatarUrl:publicUser.AvatarUrl, 
                identities:IdentitiesToDictionary(publicUser.Identities),
                isVerified: publicUser.IsApp
                );
        }

        public static UserReference ToUserReference(this THPostAuthor publicUser)
        {
            return new UserReference(
                id: publicUser.Id, 
                displayName:publicUser.DisplayName, 
                avatarUrl:publicUser.AvatarUrl
                );
        }
        
        public static UserReference ToUserReference(this THUserReference publicUser)
        {
            return new UserReference(
                id: publicUser.Id, 
                displayName:publicUser.DisplayName, 
                avatarUrl:publicUser.AvatarUrl
                );
        }

        public static ReferredUser ToReferredUser(this THPublicUser publicUser)
        {
            return new ReferredUser(
                publicProperties: publicUser.PublicProperties, 
                id: publicUser.Id, 
                displayName:publicUser.DisplayName, 
                avatarUrl:publicUser.AvatarUrl, 
                identities:IdentitiesToDictionary(publicUser.Identities),
                installationDate: DateUtils.FromUnixTime(long.Parse(publicUser.InstallDate)),
                installationChannel: publicUser.InstallProvider
            );
        }
        public static ReferralUser ToReferralUser(this THReferralUser referralUser)
        {
            var publicUser = referralUser.User;
            return new ReferralUser(
                publicProperties: publicUser.PublicProperties, 
                id: publicUser.Id, 
                displayName:publicUser.DisplayName, 
                avatarUrl:publicUser.AvatarUrl, 
                identities:IdentitiesToDictionary(publicUser.Identities),
                eventDate: DateUtils.FromUnixTime(long.Parse(referralUser.EventDate)),
                eventName: referralUser.Event,
                eventData: referralUser.CustomData
            );
        }
        
        public static THIdentity ToRpcModel(this AuthIdentity authIdentity)
        {
            return new THIdentity
            {
                Provider = authIdentity.ProviderId,
                ProviderId = authIdentity.ProviderUserId,
                AccessToken = authIdentity.AccessToken
            };
        }

        public static ReferralData ToReferralData(this THTokenInfo thTokenInfo)
        {
            var linkParams = new Dictionary<string, string>(thTokenInfo.LinkParams);
            var originalLinkParams = new Dictionary<string, string>(thTokenInfo.OriginalData);

            var token = thTokenInfo.LinkParams.GetOrDefault("$token", null);
            var provider = thTokenInfo.LinkParams.GetOrDefault("$channel", null);
            var firstMatch = bool.Parse(thTokenInfo.LinkParams.GetOrDefault("$first_match", null));
            var guaranteedMatch = bool.Parse(thTokenInfo.LinkParams.GetOrDefault("$guaranteed_match", null));
            var reinstall = bool.Parse(thTokenInfo.LinkParams.GetOrDefault("$reinstall", null));
            var firstMatchLink = bool.Parse(thTokenInfo.LinkParams.GetOrDefault("$first_match_link", null));
            var referrerUserId = thTokenInfo.LinkParams.GetOrDefault("$referrer_user_guid", null);
            
#pragma warning disable 0618
            return new ReferralData(token, referrerUserId, provider, firstMatch, guaranteedMatch, reinstall, firstMatchLink, new CustomReferralData(linkParams), linkParams, new CustomReferralData(originalLinkParams), originalLinkParams);
#pragma warning restore 0618
        }

        public static InviteChannel FromRpcModel(this THInviteProvider provider, string language)
        {
            return new InviteChannel(
                id: provider.ProviderId,
                name: provider.Name.GetOrDefault(language, provider.Name[LanguageCodes.DefaultLanguage]),
                iconImageUrl: provider.IconUrl,
                displayOrder: provider.DisplayOrder,
                isEnabled: provider.Enabled
            );
        }

        public static THAnalyticsBaseEvent ToRpcModel(this AnalyticsEvent e)
        {
            return new THAnalyticsBaseEvent
            {
                Id = e.Id,
                Name = e.Name,
                CustomProperties = e.Properties,
                DeviceTime = e.CreatedAt.ToUnixTimestamp(),
                RetryCount = 0
            };
        }

        public static THDeviceOs ToRpcModel(this OperatingSystemFamily operatingSystemFamily)
        {
#if UNITY_EDITOR
            return THDeviceOs.UNITY_EDITOR;
#else
            return new Dictionary<OperatingSystemFamily, THDeviceOs>
            {
                {OperatingSystemFamily.MacOSX, THDeviceOs.DESKTOP_MAC},
                {OperatingSystemFamily.Windows, THDeviceOs.DESKTOP_WINDOWS},
                {OperatingSystemFamily.Linux, THDeviceOs.DESKTOP_LINUX},
                {OperatingSystemFamily.Other, THDeviceOs.OTHER},
            }[operatingSystemFamily];
#endif
        }

        public static GetSocialAction FromRpcModel(this THAction thAction)
        {
            return thAction == null ? null : GetSocialAction.CreateBuilder(thAction.Type)
                    .AddActionData(thAction.Data)
                    .Build();
        }

        public static THAction ToRpcModel(this GetSocialAction thAction)
        {
            return thAction == null ? null : new THAction
            {
                Type = thAction.Type,
                Data = thAction.Data
            };
        }

        public static THNotificationsQuery ToRpcModel(this NotificationsQuery query) 
        {
            return new THNotificationsQuery
            {
                Limit = query._limit,
                Older = query._filter == NotificationsQuery.Filter.Older ? query._notificationId : null,
                Newer = query._filter == NotificationsQuery.Filter.Newer ? query._notificationId : null,
                Statuses = query._statuses.ToList(),
                Types = query._types.ToList(),
                Actions = query._actions.ToList()
            };
        }

        public static THNotificationsQuery ToRpcModel(this NotificationsCountQuery query) 
        {
            return new THNotificationsQuery
            {
                Statuses = query._statuses.ToList(),
                Types = query._types.ToList(),
                Actions = query._actions.ToList()
            };
        }

        public static Notification FromRpcModel(this THNotification thNotification)
        {
            return new Notification(
                id: thNotification.Id,
                notificationAction: thNotification.Action.FromRpcModel(),

#pragma warning disable 0618
                action: (Notification.Type) thNotification.ActionType,
#pragma warning restore 0618
                status: thNotification.Status,
                notificationType: thNotification.TypeStr,
                createdAt: thNotification.CreatedAt,
                title: thNotification.Title,
                text: thNotification.Text,
                imageUrl: thNotification.Image,
                videoUrl: thNotification.Video,
                sender: thNotification.Origin.ToUserReference(),
                actionButtons: thNotification.ActionButtons.ConvertAll(actionButton => actionButton.FromRpcModel()),
                customization: FromRpcModel(thNotification.Media)
            );
        }

        public static NotificationCustomization FromRpcModel(
            this THNotificationTemplateMedia thNotificationTemplateMedia)
        {
            if (thNotificationTemplateMedia == null)
            {
                return null;
            }

            var backgroundImageConfiguration = thNotificationTemplateMedia.BackgroundImage;
            var titleColor = thNotificationTemplateMedia.TitleColor;
            var textColor = thNotificationTemplateMedia.TextColor;
            return NotificationCustomization.WithBackgroundImageConfiguration(backgroundImageConfiguration)
                .WithTextColor(textColor)
                .WithTitleColor(titleColor);
        }

        // public static THNotificationContent ToRpcModel(this NotificationContent thNotification)
        // {
        //     return THNotification {

        //     }
        // }
        public static THUsersQuery ToRpcModel(this UsersQuery query)
        {
            return new THUsersQuery
            {
                Name = query._query,
                Limit = query._limit
            };
        }
        
        public static THReportingReason ToRpcModel(this ReportingReason reportingReason)
        {
            switch(reportingReason) 
            {
                case ReportingReason.InappropriateContent: return THReportingReason.INAPPROPRIATE_CONTENT;
                case ReportingReason.Spam: return THReportingReason.SPAM;
                default: return THReportingReason.SPAM;
            }
        }

        public static ActivityPost FromRpcModel(this THActivityPost post)
        {
            return new ActivityPost(
                id: post.Id,
                text: post.Content.Text,
                imageUrl: post.Content.ImageUrl,
                createdAt: DateUtils.FromUnixTime(post.CreatedAt),
                buttonTitle: post.Content.Button != null ? post.Content.Button.ButtonTitle : null,
                buttonAction: post.Content.Button != null ? post.Content.Button.ButtonAction: null,
                action: post.Content.Button != null ? post.Content.Button.Action.FromRpcModel(): null,
                author: post.Author.ToPostAuthor(),
                commentsCount: post.CommentsCount,
                likesCount: post.LikesCount,
                isLikedByMe: post.LikedByMe,
                stickyStart: DateUtils.FromUnixTime(post.StickyStart),
                stickyEnd: DateUtils.FromUnixTime(post.StickyEnd),
                mentions: post.Mentions.ConvertAll(mention => mention.FromRpcModel()),
                feedId: GetFeedNameFromRPC(post.FeedId)
            );
        }

        private static string GetFeedNameFromRPC(string feedName)
        {
            return ActivitiesQuery.GlobalFeed.Equals(feedName) ? feedName : feedName.Substring(2);
        }

        public static THActivityPostContent ToRpcModel(this ActivityPostContent content)
        {
            var media = content._mediaAttachment;
            if (!media.IsSupported()) {
                GetSocialLogs.W("Uploading Texture2D or video is not supported in Editor yet");
            }
            return new THActivityPostContent
            {
                Text = content._text,
                ButtonTitle = content._buttonTitle,
                ButtonAction = content._buttonAction,
                Language = GetSocial.GetLanguage(),
                ImageUrl = media.GetImageUrl(),
                VideoUrl = media.GetVideoUrl(),
                Action = content._action.ToRpcModel()
            };
        }

        public static Mention FromRpcModel(this THMention mention)
        {
            return new Mention(
                userId: mention.UserId,
                startIndex: mention.StartIdx,
                endIndex: mention.EndIdx,
                type: mention.Type
            );
        }

        public static THActivitiesQuery ToRpcModel(this ActivitiesQuery query)
        {
            return new THActivitiesQuery
            {
                Limit = query._limit,
                BeforeId = query._filter == ActivitiesQuery.Filter.Older ? query._filteringActivityId : null,
                AfterId = query._filter == ActivitiesQuery.Filter.Newer ? query._filteringActivityId : null,
                UserId = query._filterUserId,
                FriendsFeed = query._isFriendsFeed,
                Tags = query._tags.ToList()
            };
        }

        public static THPromoCode ToRpcModel(this PromoCodeBuilder builder)
        {
            var promoCode = new THPromoCode 
            {
                Code = builder._code,
                MaxClaims = (int) builder._maxClaimCount,
                CustomData = builder._data
            };
            if (builder._startDate != null) 
            {
                promoCode.ValidFrom = builder._startDate.Value.ToUnixTimestamp();
            }
            if (builder._endDate != null) 
            {
                promoCode.ValidUntil = builder._endDate.Value.ToUnixTimestamp();
            }
            return promoCode;
        }

        public static PromoCode FromRpcModel(this THPromoCode rpc)
        {
            DateTime? endDate = rpc.__isset.validUntil ? DateUtils.FromUnixTime(rpc.ValidUntil) : (DateTime?) null;
            return new PromoCode(
                code: rpc.Code,
                data: rpc.CustomData,
                maxClaimCount: (uint) rpc.MaxClaims,
                startDate: DateUtils.FromUnixTime(rpc.ValidFrom),
                endDate: endDate,
                creator: rpc.Creator.ToUserReference(),
                claimCount: (uint) rpc.NumClaims,
                enabled: rpc.Enabled,
                claimable: rpc.Claimable
           );
        }

        public static THCustomNotification ToRpcModel(this NotificationContent content)
        {
            var media = content._mediaAttachment;
            if (!media.IsSupported()) {
                GetSocialLogs.W("Uploading Texture2D or video is not supported in Editor yet");
            }
            return new THCustomNotification
            {
                Title = content._title,
                Text = content._text,
                TemplateName = content._templateName,
                TemplateData = content._templatePlaceholders,
                NewAction = content._action.ToRpcModel(),
                Image = media.GetImageUrl(),
                Video = media.GetVideoUrl(),
                ActionButtons = content._actionButtons.ConvertAll(button => button.ToRpcModel()),
                Badge = content._badge.ToRpcModel()
            };
        }

        public static THBadge ToRpcModel(this Badge badge) 
        {
            if (badge == null) 
            {
                return null;
            }
            if (badge.Value == int.MinValue)
            {
                return new THBadge { Increase = badge.Increase };
            }
            return new THBadge { Value = badge.Value };
        }

        public static THActionButton ToRpcModel(this ActionButton button)
        {
            return new THActionButton
            {
                Title = button.Title,
                ActionId = button.Id
            };
        }

        public static ActionButton FromRpcModel(this THActionButton button)
        {
            return ActionButton.Create(button.Title, button.ActionId);
        }

        public static NotificationsSummary FromRpcModel(this THNotificationsSummary summary)
        {
            return new NotificationsSummary(summary.SuccessCount);
        }

        public static bool IsSupported(this MediaAttachment media)
        {
            return media == null || media._method.Equals("imageUrl") || media._method.Equals("videoUrl");
        }

        public static string GetImageUrl(this MediaAttachment media)
        {
            return media != null && media._method.Equals("imageUrl") ? media._object as string : null;;
        }

        public static string GetVideoUrl(this MediaAttachment media)
        {
            return media != null && media._method.Equals("videoUrl") ? media._object as string : null;
        }
    }
}
#endif
