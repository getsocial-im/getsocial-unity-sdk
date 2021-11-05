#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Thrift.Collections;
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
            return new ConflictUser
            {
                PublicProperties = conflictUser.PublicProperties,
                Id = conflictUser.Id,
                Identities = IdentitiesToDictionary(conflictUser.Identities),
                Verified = false,
                AvatarUrl = conflictUser.AvatarUrl,
                DisplayName = conflictUser.DisplayName,
                BanInfo = ConvertBanInfo(conflictUser.InternalPrivateProperties),
                PrivateProperties = conflictUser.PrivateProperties
            };
        }

        public static CurrentUser ToCurrentUser(this THPrivateUser currentUser)
        {
            return new CurrentUser
            {
                PublicProperties = currentUser.PublicProperties,
                Id = currentUser.Id,
                Identities = IdentitiesToDictionary(currentUser.Identities),
                Verified = false,
                AvatarUrl = currentUser.AvatarUrl,
                DisplayName = currentUser.DisplayName,
                BanInfo = ConvertBanInfo(currentUser.InternalPrivateProperties),
                PrivateProperties = currentUser.PrivateProperties
            };
        }

        private static BanInfo ConvertBanInfo(Dictionary<string, string> privateProperties)
        {
            if (privateProperties == null)
            {
                return null;
            }

            if (!privateProperties.ContainsKey("expiry"))
            {
                return null;
            }

            var expiry = long.Parse(privateProperties["ban_expiry"]);
            var reason = privateProperties.ContainsKey("ban_reason") ? privateProperties["ban_reason"] : null;
            return new BanInfo
            {
                Expiration = expiry,
                Reason = reason
            };
        }

        public static THPrivateUser ToRPC(this UserUpdate userUpdate)
        {
            // todo add media
            return new THPrivateUser
            {
                AvatarUrl = userUpdate.AvatarUrl,
                DisplayName = userUpdate.DisplayName,
                PublicProperties = userUpdate._publicProperties,
                PrivateProperties = userUpdate._privateProperties,
            };
        }

        public static SuggestedFriend ToSuggestedFriend(this THSuggestedFriend suggestedFriend)
        {
            var user = suggestedFriend.User;
            return new SuggestedFriend {
                PublicProperties = user.PublicProperties,
                Id = user.Id,
                Identities = IdentitiesToDictionary(user.Identities),
                Verified = false,
                AvatarUrl = user.AvatarUrl,
                DisplayName = user.DisplayName,
                MutualFriendsCount = suggestedFriend.MutualFriends
            };
        }

        public static Dictionary<string, bool> FromRPCModel(this AreFriendsResponse response, THashSet<string> sourceIds)
        {
            var result = response.Result;
            sourceIds.ToList().ForEach((sourceId) => {
                if (!result.ContainsKey(sourceId))
                {
                    result.Add(sourceId, false);
                }
            });
            return result;
        }

        public static PagingResult<T> ToPagingResult<T>(List<T> entries, int offset, int limit)
        {
            var next = entries.Count < limit ? "" : (offset + limit).ToString();
            return new PagingResult<T>
            {
                Entries = entries,
                NextCursor = next
            };
        }

        public static ReferralUser FromRPCModel(this THReferralUser thReferralUser)
        {
            var user = thReferralUser.User;
            return new ReferralUser
            {
                PublicProperties = user.PublicProperties,
                Id = user.Id,
                Identities = IdentitiesToDictionary(user.Identities),
                Verified = false,
                AvatarUrl = user.AvatarUrl,
                DisplayName = user.DisplayName,
                EventData = thReferralUser.CustomData ?? new Dictionary<string, string>(), 
                Event = thReferralUser.Event,
                EventDate = long.Parse(thReferralUser.EventDate),
            };
        }

        public static THIdentity ToRpcModel(this Identity identity)
        {
            return new THIdentity
            {
                Provider = identity.ProviderId,
                ProviderId = identity.ProviderUserId,
                AccessToken = identity.AccessToken
            };
        }

        public static InviteChannel FromRpcModel(this THInviteProvider provider, string language)
        {
            return new InviteChannel{
                Id = provider.ProviderId,
                IconImageUrl = provider.IconUrl,
                DisplayOrder = provider.DisplayOrder,
                Name = provider.Name.ContainsKey(language) ? provider.Name[language] : provider.Name[LanguageCodes.DefaultLanguage]
            };
        }

        public static THAnalyticsBaseEvent ToRpcModel(this AnalyticsEvent e)
        {
            return new THAnalyticsBaseEvent
            {
                Id = e.Id,
                Name = e.Name,
                CustomProperties = e.Properties,
                DeviceTimeType = THDeviceTimeType.SERVER_TIME,
                DeviceTime = e.CreatedAt.ToUnixTimestamp(),
                RetryCount = 0,
                IsCustom = e.IsCustom
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
            return GetSocialAction.Create(thAction.Type, thAction.Data ?? new Dictionary<string, string>());
        }

        public static THAction ToRpcModel(this GetSocialAction thAction)
        {
            return thAction == null ? null : new THAction
            {
                Type = thAction.Type,
                Data = thAction.Data
            };
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

        public static THActionButton ToRpcModel(this NotificationButton button)
        {
            return new THActionButton
            {
                Title = button.Title,
                ActionId = button.ActionId
            };
        }

        public static NotificationButton FromRpcModel(this THActionButton button)
        {
            return NotificationButton.Create(button.Title, button.ActionId);
        }

        #region SDK7

        public static GetUsersRequest ToRpc(this PagingQuery<UsersQuery> query)
        {
            var request = query.Query.ToRpc();
            request.Pagination = query.ToPagination();
            return request;
        }

        public static GetVotesRequest ToRpc(this PagingQuery<VotesQuery> query)
        {
            var request = query.Query.ToRpc();
            request.Pagination = query.ToPagination();
            return request;
        }

        public static GetActivitiesV2Request ToRpc(this PagingQuery<ActivitiesQuery> query)
        {
            var request = query.Query.ToRpc();
            request.Pagination = query.ToPagination();
            return request;
        }
        
        public static GetAnnouncementsRequest ToRpc(this AnnouncementsQuery query)
        {
            return new GetAnnouncementsRequest
            {
                Target = query.Ids.ToRpc(),
                WithPolls = GetPollFilterType(query.InternalPollStatus)
            };
        }
        public static GetReactionsRequest ToRpc(this PagingQuery<ReactionsQuery> query)
        {
            var request = query.Query.ToRpc();
            request.Pagination = query.ToPagination();
            return request;
        }

        public static GetReactionsRequest ToRpc(this ReactionsQuery query)
        {
            return new GetReactionsRequest
            {
                Reaction = query.Reaction, Target = query.Ids.ToRpc()
            };
        }

        public static FindTagsRequest ToRpc(this TagsQuery query)
        {
            return new FindTagsRequest
            {
                SearchTerm = query.Query,
                Target = query.Target?.Ids.ToRpc(),
                IsTrending = query.InternalTrending,
                //OrderBy = query.InternalSortOrder
            };
        }

        public static GetVotesRequest ToRpc(this VotesQuery query)
        {
            return new GetVotesRequest
            {
                Target = query.Ids.ToRpc(),
                OptionId = query.PollOptionId
            };
        }

        public static GetActivitiesV2Request ToRpc(this ActivitiesQuery query)
        {
            return new GetActivitiesV2Request
            {
                Target = query.Ids.ToRpc(),
                WithPolls = GetPollFilterType(query.InternalPollStatus),
                IsTrending = query.InternalTrending,
                //OrderBy = query.InternalSortOrder
            };
        }

        private static AFPollFilterType GetPollFilterType(int pollStatus)
        {
            switch (pollStatus) {
                case PollStatus.All:
                    return AFPollFilterType.all;
                case PollStatus.WithPoll:
                    return AFPollFilterType.onlyPolls;
                case PollStatus.WithPollVotedByMe:
                    return AFPollFilterType.onlyPollsVotedByMe;
                case PollStatus.WithPollNotVotedByMe:
                    return AFPollFilterType.onlyPollsNotVotedByMe;
                case PollStatus.WithoutPoll:
                    return AFPollFilterType.onlyWithoutPolls;
                default:
                    return AFPollFilterType.all;
            }
        }

        public static AFButton ToRpc(this ActivityButton button)
        {
            return new AFButton
            {
                Action = button.Action.ToRpcModel(),
                ButtonTitle = button.Title
            };
        }

        public static CreateActivityRequest ToRpc(this ActivityContent content, string lang, PostActivityTarget target)
        {
            return new CreateActivityRequest
            {
                Target = target.Ids.ToRpc(),
                Properties = content.Properties,
                Content = new Dictionary<string, AFContent>
                {
                    { lang, new AFContent
                    {
                        Text = content.Text,
                        Button = content.Button?.ToRpc(),
                        Attachments = content.Attachments.ConvertAll(it => it.ToRpc())
                    } }
                },
                Poll = content.Poll?.ToRpc(lang)
            };
        }

        public static UpdateActivityRequest ToUpdateRpc(this ActivityContent content, string lang, string activityId)
        {
            return new UpdateActivityRequest
            {
                ActivityId = activityId,
                Properties = content.Properties,
                Content = new Dictionary<string, AFContent>
                {
                    {lang, new AFContent
                    {
                        Text = content.Text,
                        Button = content.Button?.ToRpc(),
                        Attachments = content.Attachments.ConvertAll(it => it.ToRpc())
                    }}
                },
                Poll = content.Poll?.ToRpc(lang)
            };
        }

        public static AFPollContent ToRpc(this PollContent pollContent, string lang)
        {
            var retValue = new AFPollContent();
            retValue.AllowMultiVotes = pollContent.AllowMultipleVotes;
            retValue.PollOptions = pollContent.Options.ConvertAll(it => it.ToRpc(lang));
            if (pollContent.EndDate != null)
            {
                retValue.EndsAt = pollContent.EndDate.Value.ToUnixTimestamp();
            }
            return retValue;
        }

        public static AFPollOption ToRpc(this PollOptionContent pollOptionContent, string lang)
        {
            return new AFPollOption
            {
                Id = pollOptionContent.OptionId,
                Content = new Dictionary<string, AFPollOptionContent>
                {
                    { lang, new AFPollOptionContent
                        {
                            Text = pollOptionContent.Text,
                            Attachment = pollOptionContent.Attachment != null ? pollOptionContent.Attachment.ToRpc() : null
                        }
                    }
                },
            };
        }

        public static MediaAttachment FromRpc(this AFAttachment attachment)
        {
            return new MediaAttachment
            {
                ImageUrl = attachment.ImageUrl,
                VideoUrl = attachment.VideoUrl
            };
        }
        public static AFAttachment ToRpc(this MediaAttachment attachment)
        {
            return new AFAttachment
            {
                ImageUrl = attachment.ImageUrl,
                VideoUrl = attachment.VideoUrl
            };
        }
        
        public static GetFriendsRequest ToRpc(this PagingQuery<FriendsQuery> query)
        {
            var request = query.Query.ToRpc();
            request.Pagination = query.ToPagination();
            return request;
        }

        public static GetFriendsRequest ToRpc(this FriendsQuery query)
        {
            return new GetFriendsRequest
            {
                UserId = query.UserId.ToRpc()
            };
        }

        public static SGEntity ToRpc(this CommunitiesIds ids)
        {
            if (ids == null || ids.Type == null)
            {
                return null;
            }
            return new SGEntity
            {
                EntityType = (int) ids.Type, Id = ids.Ids[0]
            };
        }

        public static SuggestedFriend FromRPCModel(this THSuggestedFriend friend)
        {
            var user = friend.User;
            return new SuggestedFriend
            {
                PublicProperties = user.PublicProperties,
                Id = user.Id,
                Identities = IdentitiesToDictionary(user.Identities),
                Verified = false,
                AvatarUrl = user.AvatarUrl,
                DisplayName = user.DisplayName, 
                MutualFriendsCount = friend.MutualFriends
            };
        }
        public static PagingResult<SuggestedFriend> FromRPCModel(this GetSuggestedFriendsResponse response)
        {
            return FromRPCModel(response.Users, response.NextCursor, FromRPCModel);
        }
        
        public static PagingResult<User> FromRPCModel(this GetFriendsResponse response)
        {
            return FromRPCModel(response.Users, response.NextCursor, FromRPCModel);
        }

        public static PagingResult<User> FromRPCModel(this GetUsersResponse response)
        {
            return FromRPCModel(response.Users, response.NextCursor, FromRPCModel);
        }

        public static PagingResult<UserVotes> FromRPCModel(this GetVotesResponse response)
        {
            return FromRPCModel(response.Votes, response.NextCursor, FromRPCModel);
        }

        public static UserVotes FromRPCModel(this AFPollVote vote)
        {
            return new UserVotes { User = vote.Creator.FromRPCModel(), VotesList = vote.OptionIds};
        }


        public static PagingResult<Activity> FromRPCModel(this GetActivitiesV2Response response)
        {
            var users = response.Authors;
            var sources = response.EntityDetails;
            return FromRPCModel(response.Data, response.NextCursor, af =>
            {
                var id = af.Author.PublicUser?.Id;
                var user = id != null && (users != null && users.ContainsKey(id)) ? users[id] : null;
                var source = FindSource(sources, af.Source);
                return FromRPCModel(af, user, source);
            });
        }

        private static AFEntityReference FindSource(List<AFEntityReference> list, AFEntityReference source)
        {
            if (source == null || source.Id == null)
            {
                return null;
            }

            foreach (var reference in list) 
            {
                if (source.Id.Id.Equals(reference.Id.Id) && source.Id.EntityType == reference.Id.EntityType)
                {
                    return reference;
                }
            }

            return source;
        }

        public static Activity FromRPCModel(this AFActivity activity, THPublicUser user = null, AFEntityReference source = null)
        {
            var content = activity.Content.FirstValue(new AFContent());
            return new Activity
            {
                Id = activity.Id,
                Type = activity.ContentType, 
                Announcement = activity.IsAnnouncement, 
                Button = content.Button.FromRPCModel(), 
                Author = activity.Author.FromRPCModel(user),
                Properties = activity.Properties ?? new Dictionary<string, string>(),
                CreatedAt = activity.CreatedAt,
                //ViewCount = activity.Reactions.ViewCount,
                CommentsCount = activity.Reactions.CommentCount,
                MediaAttachments = (content.Attachments ?? new List<AFAttachment>()).ConvertAll(FromRpc),
                Text = content.Text,
                ReactionsCount = activity.Reactions.ReactionCount,
                MyReactionsList = activity.Reactions.MyReactions ?? new List<string>(), 
                Mentions = activity.Mentions.FirstValue(new List<AFMention>()).ConvertAll(FromRPCModel), 
                Source = (source ?? activity.Source).FromRPCModel(),
                Status = activity.Status ?? "",
                Poll = activity.Poll.FromRPCModel(),
                Popularity = activity.Score
            };
        }

        public static Poll FromRPCModel(this AFPollContent pollContent)
        {
            if (pollContent == null)
            {
                return null;
            }
            return new Poll
            {
                AllowMultipleVotes = pollContent.AllowMultiVotes,
                EndDate = pollContent.EndsAt,
                TotalVotes = pollContent.VoteCount,
                KnownVoters = pollContent.KnownVoters != null ? pollContent.KnownVoters.ConvertAll(FromRPCModel) : new List<UserVotes>(),
                Options = pollContent.PollOptions != null ? pollContent.PollOptions.ConvertAll(FromRPCModel) : new List<PollOption>()
            };
        }

        public static PollOption FromRPCModel(this AFPollOption pollOption)
        {
            var content = pollOption.Content.Values.First();
            return new PollOption
            {
                OptionId = pollOption.Id,
                Text = content.Text,
                Attachment = content.Attachment != null ? content.Attachment.FromRpc() : null,
                VoteCount = pollOption.VoteCount,
                IsVotedByMe = pollOption.IsVotedByMe
            };
        }

        public static CommunitiesEntity FromRPCModel(this AFEntityReference source)
        {
            return new CommunitiesEntity
            {
                Type = (CommunitiesEntityType) source.Id.EntityType, 
                Id = source.Id.Id,
                Title = source.Title.FirstValue(),
                AvatarUrl = source.AvatarUrl,
                FollowersCount = source.FollowersCount,
                IsFollower = source.IsFollower,
                AvailableActions = ConvertAllowableActions(source.AllowedActions)
            };
        }

        public static T FirstValue<K, T>(this Dictionary<K, T> dictionary, T def)
        {
            return dictionary == null || dictionary.Count == 0 ? def : dictionary.First().Value;
        }

        public static string FirstValue<K>(this Dictionary<K, string> dictionary)
        {
            return dictionary.FirstValue("");
        }

        public static Mention FromRPCModel(this AFMention mention)
        {
            return new Mention {UserId = mention.UserId, StartIndex = mention.StartIdx, EndIndex = mention.EndIdx, Type = mention.MentionType == 0 ? Mention.MentionType.App : Mention.MentionType.User};
        }

        public static ActivityButton FromRPCModel(this AFButton button)
        {
            if (button == null)
            {
                return null;
            }
            return ActivityButton.Create(button.ButtonTitle, button.Action.FromRpcModel());
        }

        public static User FromRPCModel(this THPublicUser user)
        {
            return new User
            {
                PublicProperties = user.PublicProperties,
                Id = user.Id,
                Identities = IdentitiesToDictionary(user.Identities),
                Verified = false,
                AvatarUrl = user.AvatarUrl,
                DisplayName = user.DisplayName
            };
        }
        
        public static User FromRPCModel(this THCreator creator, THPublicUser user = null)
        {
            if (creator.IsApp)
            {
                return new User
                {
                    Id = "app",
                    AvatarUrl = GetSocialStateController.Info.IconUrl,
                    DisplayName = GetSocialStateController.Info.Name,
                    Identities = new Dictionary<string, string>(),
                    Verified = true,
                    PublicProperties = new Dictionary<string, string>()
                };
            }

            if (user == null)
            {
                user = creator.PublicUser;
            }

            if (user == null)
            {
                return null;
            }
            return new User
            {
                PublicProperties = user.PublicProperties,
                Id = user.Id,
                Identities = IdentitiesToDictionary(user.Identities),
                Verified = creator.IsVerified,
                AvatarUrl = user.AvatarUrl,
                DisplayName = user.DisplayName
            };
        }

        public static GetUsersRequest ToRpc(this UsersQuery query)
        {
            return new GetUsersRequest
            {
                SearchTerm = query.Query,
                FollowedByUserId = query.FollowedBy.ToRpc()
            };
        }

        public static string ToRpc(this UserId userId)
        {
            return userId?.AsString();
        }

        public static CommunitiesAction[] CommunitiesActionFromRPC(int rawValue)
        {
            switch (rawValue)
            {
                case 0:
                    return new CommunitiesAction[] { CommunitiesAction.Post };
                default:
                    return new CommunitiesAction[] { CommunitiesAction.Comment, CommunitiesAction.React };
            }
        }

        public static CommunitiesSettings FromRPC(this SGSettings rpcSettings)
        {
            var permissions = new Dictionary<CommunitiesAction, MemberRole>();
            foreach (int key in rpcSettings.Permissions.Keys)
            {
                var convertedKeys = CommunitiesActionFromRPC(key);
                var convertedValue = MemberRoleFromRPCModel(rpcSettings.Permissions[key]);
                foreach (CommunitiesAction action in convertedKeys)
                {
                    permissions[action] = convertedValue;
                }
            }
            return new CommunitiesSettings
            {
                Properties = rpcSettings.Properties,
                AllowedActions = ConvertAllowableActions(rpcSettings.AllowedActions),
                IsPrivate = rpcSettings.IsPrivate,
                IsDiscoverable = rpcSettings.IsDiscoverable,
                Permissions = permissions,
                Labels = rpcSettings.Labels,
            };
        }

        public static Dictionary<CommunitiesAction, bool> ConvertAllowableActions(this Dictionary<int, bool> actions)
        {
            var allowedActions = new Dictionary<CommunitiesAction, bool>();
            if (actions != null) {
                foreach (var action in actions)
                {
                    var convertedActions = CommunitiesActionFromRPC(action.Key);
                    for (int i = 0; i<convertedActions.Length; i++)
                    {
                        allowedActions[convertedActions[i]] = action.Value;
                    }
                }
            }

            return allowedActions;
        }

        public static GetTopicsRequest ToRPC(this TopicsQuery query)
        {
            var request = new GetTopicsRequest();
            request.FollowedByUserId = query.FollowerId.ToRpc();
            request.SearchTerm = query.SearchTerm;
            request.IsTrending = query.InternalTrending;
            //request.OrderBy = query.InternalSortOrder;
            request.Labels = query.Labels;
            request.Properties = query.Properties;
            return request;
        }

        public static GetTopicsRequest ToRPC(this PagingQuery<TopicsQuery> pagingQuery)
        {
            var request = pagingQuery.Query.ToRPC();
            request.Pagination = CreatePagination(pagingQuery);
            return request;
        }
        
        public static GetNotificationsRequest ToRPC(this PagingQuery<NotificationsQuery> pagingQuery)
        {
            var request = pagingQuery.Query.ToRPC();
            request.Pagination = CreatePagination(pagingQuery);
            return request;
        }

        public static GetNotificationsRequest ToRPC(this NotificationsQuery query)
        {
            return new GetNotificationsRequest
            {
                Statuses = query.Statuses,
                Actions = query.Actions,
                Types = query.Types
            };
        }

        public static GetGroupMembersRequest ToRPC(this PagingQuery<MembersQuery> pagingQuery)
        {
            var request = pagingQuery.Query.ToRPC();
            request.Pagination = CreatePagination(pagingQuery);
            return request;
        }

        public static GetGroupMembersRequest ToRPC(this MembersQuery query)
        {
            var request = new GetGroupMembersRequest();
            request.GroupId = query.GroupId;
            if (query.Status != null)
            {
                request.Status = (int)query.Status;
            }
            if (query.Role != null)
            {
                request.Role = (int)query.Role;
            }
            return request;
        }

        public static GetGroupsRequest ToRPC(this PagingQuery<GroupsQuery> pagingQuery)
        {
            var request = pagingQuery.Query.ToRPC();
            request.Pagination = CreatePagination(pagingQuery);
            return request;
        }

        public static GetGroupsRequest ToRPC(this GroupsQuery query)
        {
            var request = new GetGroupsRequest();
            request.FollowedByUserId = query.FollowerId.ToRpc();
            request.MemberUserId = query.MemberId.ToRpc();
            request.SearchTerm = query.SearchTerm;
            request.IsTrending = query.InternalTrending;
            //request.OrderBy = query.InternalSortOrder;
            request.Labels = query.Labels;
            request.Properties = query.Properties;
            return request;
        }

        public static UpdateGroupMembersRequest ToRPC(this UpdateGroupMembersQuery query)
        {
            var request = new UpdateGroupMembersRequest();
            request.GroupId = query.GroupId;
            request.UserIds = query.UserIdList.AsString();
            request.Status = (int)query.Status;
            request.Role = (int)query.Role;
            request.InvitationToken = query.InvitationToken;
            return request;
        }

        public static RemoveGroupMembersRequest ToRPC(this RemoveGroupMembersQuery query)
        {
            var request = new RemoveGroupMembersRequest();
            request.GroupId = query.GroupId;
            request.UserIds = query.UserIdList.AsString();
            return request;
        }

        public static FollowEntitiesRequest ToRPC(this FollowQuery query)
        {
            var request = new FollowEntitiesRequest();
            request.EntityIds = new Thrift.Collections.THashSet<string>();
            request.EntityIds.AddAll(query.Ids.Ids);
            request.EntityType = (int)query.Ids.Type;
            return request;
        }

        public static UnfollowEntitiesRequest ToUnfollowRPC(this FollowQuery query)
        {
            var request = new UnfollowEntitiesRequest();
            request.EntityIds = new Thrift.Collections.THashSet<string>();
            request.EntityIds.AddAll(query.Ids.Ids);
            request.EntityType = (int)query.Ids.Type;
            return request;
        }


        public static IsFollowingRequest ToIsFollowingRPC(this FollowQuery query)
        {
            var request = new IsFollowingRequest();
            request.EntityIds = new Thrift.Collections.THashSet<string>();
            request.EntityIds.AddAll(query.Ids.Ids);
            request.EntityType = (int)query.Ids.Type;
            return request;
        }

        public static GetEntityFollowersRequest ToRPC(this PagingQuery<FollowersQuery> pagingQuery)
        {
            var request = pagingQuery.Query.ToRPC();
            request.Pagination = CreatePagination(pagingQuery);
            return request;
        }

        public static GetEntityFollowersRequest ToRPC(this FollowersQuery query)
        {
            var request = new GetEntityFollowersRequest();
            request.Id = new SGEntity();
            request.Id.Id = query.Ids.Ids.First() ;
            request.Id.EntityType = (int)query.Ids.Type;
            return request;
        }

        public static Pagination CreatePagination<T>(PagingQuery<T> pagingQuery)

        {
            var pagination = new Pagination();
            pagination.Limit = pagingQuery.Limit;
            pagination.NextCursor = pagingQuery.NextCursor;
            return pagination;
        }

        public static Topic FromRPCModel(this SGTopic rpcTopic)
        {
            var topic = new Topic();
            topic.Id = rpcTopic.Id;
            topic.AvatarUrl = rpcTopic.AvatarUrl;
            topic.CreatedAt = rpcTopic.CreatedAt;
            topic.Description =  new LocalizableText(rpcTopic.TopicDescription).LocalizedValue();
            topic.FollowersCount = rpcTopic.FollowersCount;
            topic.IsFollowedByMe = rpcTopic.IsFollower;
            topic.Title = new LocalizableText(rpcTopic.Title).LocalizedValue();
            topic.Settings = rpcTopic.Settings.FromRPC();
            topic.UpdatedAt = rpcTopic.UpdatedAt;
            topic.Popularity = rpcTopic.Score;
            return topic;
        }

        public static SGChatMessageContent ToRPCModel(this ChatMessageContent content)
        {
            var sgMessageContent = new SGChatMessageContent();
            sgMessageContent.Text = content.Text;
            sgMessageContent.Properties = content.Properties;
            sgMessageContent.Attachments = new List<SGChatMessageAttachment>();
            if (content.Attachments != null)
            {
                content.Attachments.ForEach((MediaAttachment obj) => obj.ToRPCModel());
            }
            return sgMessageContent;
        }

        public static ChatMessage FromRPCModel(this SGChatMessage response)
        {
            var chatMessage = new ChatMessage();
            chatMessage.Id = response.Id;
            chatMessage.Text = response.Content.Text;
            chatMessage.SentAt = response.SentAt;
            chatMessage.Properties = response.Properties;
            chatMessage.MediaAttachments = new List<MediaAttachment>();
            if (response.Content.Attachments != null)
            {
                response.Content.Attachments.ForEach((SGChatMessageAttachment obj) => chatMessage.MediaAttachments.Add(obj.FromRPCModel()));
            }
            if (response.Author.PublicUser.Id.Equals(GetSocial.GetCurrentUser().Id))
            {
                chatMessage.Author = GetSocial.GetCurrentUser();
            } else
            {
                chatMessage.Author = response.Author.FromRPCModel();
            }
            return chatMessage;
        }

        public static ChatMessagesPagingResult FromRPCModel(this GetChatMessagesResponse response)
        {
            var result = new ChatMessagesPagingResult();
            result.NextMessagesCursor = response.NextCursor;
            result.PreviousMessagesCursor = response.PreviousCursor;
            result.RefreshCursor = response.PollingCursor;
            result.Messages = new List<ChatMessage>();
            response.Messages.ForEach((SGChatMessage obj) => result.Messages.Add(obj.FromRPCModel()));
            return result;
        }

        public static Chat FromRPCModel(this SGChat rpcChat)
        {
            var chat = new Chat();
            chat.Id = rpcChat.Id;
            chat.Title = rpcChat.Title;
            chat.AvatarUrl = rpcChat.AvatarUrl;
            chat.CreatedAt = rpcChat.CreatedAt;
            chat.UpdatedAt = rpcChat.UpdatedAt;
            chat.MembersCount = rpcChat.MembersCount;
            chat.LastMessage = rpcChat.LastMessage.FromRPCModel();
            chat.OtherMember = rpcChat.OtherMember.FromRPCModel();
            return chat;
        }

        public static Chat FromRPCModel(this GetChatResponse rpcResponse)
        {
            return rpcResponse.Chat.FromRPCModel();
        }

        public static SGChatPagination ToRPCModel(this ChatMessagesPagingQuery pagingQuery)
        {
            var pagination = new SGChatPagination();
            if (pagingQuery.NextMessages != null && pagingQuery.NextMessages.Length != 0)
            {
                pagination.Cursor = pagingQuery.NextMessages;
            }
            if (pagingQuery.PreviousMessages != null && pagingQuery.PreviousMessages.Length != 0)
            {
                pagination.Cursor = pagingQuery.PreviousMessages;
            }
            pagination.Limit = pagingQuery.Limit;
            return pagination;
        }

        public static SGChatMessageAttachment ToRPCModel(this MediaAttachment mediaAttachment)
        {
            var attachment = new SGChatMessageAttachment();
            attachment.ImageURL = mediaAttachment.ImageUrl;
            attachment.VideoURL = mediaAttachment.VideoUrl;
            return attachment;
        }

        public static MediaAttachment FromRPCModel(this SGChatMessageAttachment sGChatMessageAttachment)
        {
            var attachment = new MediaAttachment();
            attachment.ImageUrl = sGChatMessageAttachment.ImageURL;
            attachment.VideoUrl = sGChatMessageAttachment.VideoURL;
            return attachment;
        }

        public static PagingResult<Chat> FromRPCModel(this GetChatsResponse response)
        {
            return FromRPCModel(response.Chats, response.NextCursor, FromRPCModel);
        }

        public static CreateGroupRequest ToRPC(this GroupContent content, string sdkLanguage)
        {
            var request = new CreateGroupRequest();
            request.Id = content.Id;
            if (content.Title != null)
            {
                request.Title = new Dictionary<string, string> { { sdkLanguage, content.Title } };
            }
            if (content.Description != null)
            {
                request.GroupDescription = new Dictionary<string, string> { { sdkLanguage, content.Description } };
            }
            if (content.Avatar != null)
            {
                request.AvatarUrl = content.Avatar.ImageUrl;
            }
            request.IsDiscoverable = content.IsDiscoverable;
            request.IsPrivate = content.IsPrivate;
            var permissions = new Dictionary<int, int>();
            foreach (var entry in content.Permissions)
            {
                permissions[(int)entry.Key] = (int)entry.Value;
            }
            request.Permissions = permissions;
            request.Properties = content.Properties;
            request.Labels = content.Labels;
            return request;
        }

        public static UpdateGroupRequest ToUpdateRPC(this GroupContent content, string sdkLanguage)
        {
            var request = new UpdateGroupRequest();
            request.Id = content.Id;
            if (content.Title != null)
            {
                request.Title = new Dictionary<string, string> { { sdkLanguage, content.Title } };
            }
            if (content.Description != null)
            {
                request.GroupDescription = new Dictionary<string, string> { { sdkLanguage, content.Description } };
            }
            if (content.Avatar != null)
            {
                request.AvatarUrl = content.Avatar.ImageUrl;
            }
            request.IsDiscoverable = content.IsDiscoverable;
            request.IsPrivate = content.IsPrivate;
            var permissions = new Dictionary<int, int>();
            foreach (var entry in content.Permissions)
            {
                permissions[(int)entry.Key] = (int)entry.Value;
            }
            request.Permissions = permissions;
            request.Properties = content.Properties;
            request.Labels = content.Labels;
            return request;
        }

        public static Group FromRPCModel(this SGGroup rpcGroup)
        {
            var group = new Group();
            group.Id = rpcGroup.Id;
            group.Title = new LocalizableText(rpcGroup.Title).LocalizedValue();
            group.Description = new LocalizableText(rpcGroup.GroupDescription).LocalizedValue();
            group.AvatarUrl = rpcGroup.AvatarUrl;
            group.CreatedAt = rpcGroup.CreatedAt;
            group.FollowersCount = rpcGroup.FollowersCount;
            group.IsFollowedByMe = rpcGroup.IsFollower;
            group.MembersCount = rpcGroup.MembersCount;
            group.Membership = rpcGroup.Membership.FromRPCModel();
            group.Settings = rpcGroup.Settings.FromRPC();
            group.UpdatedAt = rpcGroup.UpdatedAt;
            group.Popularity = rpcGroup.Score;
            return group;
        }

        public static GroupMember FromRPCModel(this SGGroupMember rpcGroupMember)
        {
            var user = rpcGroupMember.User;
            return new GroupMember
            {
                PublicProperties = user.PublicProperties,
                Id = user.Id,
                Identities = IdentitiesToDictionary(user.Identities),
                Verified = false,
                AvatarUrl = user.AvatarUrl,
                DisplayName = user.DisplayName,
                Membership = rpcGroupMember.Membership.FromRPCModel()
            };

        }

        public static Pagination ToPagination<T>(this PagingQuery<T> query)
        {
            return new Pagination
            {
                Limit = query.Limit,
                NextCursor = query.NextCursor
            };
        }

        public static PagingResult<T> FromRPCModel<R, T>(List<R> entries, string next, Converter<R, T> convert)
        {
            return new PagingResult<T>
            {
                Entries = (entries ?? new List<R>()).ConvertAll(convert), NextCursor = next ?? ""
            };
        }

        public static PagingResult<Notification> FromRPCModel(this GetNotificationsResponse response)
        {
            var users = response.Authors;
            return FromRPCModel(response.Notifications, response.NextCursor, notification =>
            {
                var id = notification.Sender.PublicUser?.Id;
                var user = id != null && users.ContainsKey(id) ? users[id] : null;
                return FromRPCModel(notification, user);
            });
        }

        private static Notification FromRPCModel(PNNotification notification, THPublicUser user)
        {
            if (user == null)
            {
                user = notification.Sender.PublicUser;
            }

            return new Notification
            {
                Id = notification.Id,
                Customization = new NotificationCustomization
                {
                    BackgroundImageConfiguration = notification.Media?.BackgroundImage,
                    TextColor = notification.Media?.TextColor,
                    TitleColor = notification.Media?.TitleColor
                },
                ActionButtons = (notification.ActionButtons ?? new List<THActionButton>()).ConvertAll(it => it.FromRpcModel()),
                Attachment = null, // todo
                CreatedAt = notification.CreatedAt,
                Action = notification.Action?.FromRpcModel(),
                Sender = user.FromRPCModel(),
                Type = notification.Type,
                Status = notification.Status,
                Text = notification.Text,
                Title = notification.Title
            };
        }

        public static PagingResult<Topic> FromRPCModel(this GetTopicsResponse response)
        {
            return FromRPCModel(response.Topics, response.NextCursor, FromRPCModel);
        }

        public static PagingResult<GroupMember> FromRPCModel(this GetGroupMembersResponse response)
        {
            return FromRPCModel(response.Members, response.NextCursor, FromRPCModel);
        }

        public static PagingResult<Group> FromRPCModel(this GetGroupsResponse response)
        {
            return FromRPCModel(response.Groups, response.NextCursor, FromRPCModel);
        }

        public static PagingResult<User> FromRPCModel(this GetEntityFollowersResponse response)
        {
            return FromRPCModel(response.Followers, response.NextCursor, FromRPCModel);
        }
        
        public static PagingResult<UserReactions> FromRPCModel(this GetReactionsResponse response)
        {
            return FromRPCModel(response.Reactions, response.NextCursor, FromRPCModel);
        }

        public static UserReactions FromRPCModel(this AFReaction reaction)
        {
            return new UserReactions { User = reaction.Creator.FromRPCModel(), ReactionsList = reaction.Reactions};
        }

        public static Membership FromRPCModel(this SGMembershipInfo rpcMembership)
        {
            if (rpcMembership.CreatedAt == 0)
            {
                return null;
            }
            var info = new Membership();
            info.CreatedAt = rpcMembership.CreatedAt;
            info.InvitationToken = rpcMembership.InvitationToken;
            info.Role = MemberRoleFromRPCModel(rpcMembership.Role);
            info.Status = MemberStatusFromRPCModel(rpcMembership.Status);
            return info;
        }
        
        public static MemberRole MemberRoleFromRPCModel(int rawValue)
        {
            switch (rawValue)
            {
                case 0:
                    return MemberRole.Owner;
                case 1:
                    return MemberRole.Admin;
                case 3:
                    return MemberRole.Member;
                case 4:
                    return MemberRole.Follower;
                case 5:
                    return MemberRole.Everyone;
            }
            throw new ArgumentException();
        }

        public static MemberStatus MemberStatusFromRPCModel(int rawValue)
        {
            switch (rawValue)
            {
                case 0:
                    return MemberStatus.ApprovalPending;
                case 1:
                    return MemberStatus.InvitationPending;
                case 2:
                    return MemberStatus.Member;
            }
            throw new ArgumentException();
        }

        public static Dictionary<string, Membership> FromRPCModel(this AreGroupMembersResponse response)
        {
            var result = new Dictionary<string, Membership>();
            foreach (var entry in response.Result)
            {
                var membership = entry.Value.FromRPCModel();
                if (membership != null)
                {
                    result[entry.Key] = membership;
                }
            }
            return result;
        }

        public static Dictionary<string, bool> FromRPCModel(this IsFollowingResponse response, THashSet<string> sourceIds)
        {
            var result = response.Result;
            sourceIds.ToList().ForEach((sourceId) => {
                if (!result.ContainsKey(sourceId))
                {
                    result.Add(sourceId, false);
                }
            });
            return result;
        }

        #endregion

        #region Promo codes

        public static CreatePromoCodeRequest ToRPC(this PromoCodeContent content)
        {
            var request = new CreatePromoCodeRequest();

            var code = new SIPromoCode();
            code.MaxClaims = (int)content.MaxClaimCount;
            code.Code = content.Code;
            code.Properties = content.Data;
            if (content._startDate != 0) 
            {
                code.ValidFrom = content._startDate;
            }
            if (content._endDate != 0) 
            {
                code.ValidUntil = content._endDate;
            }

            request.Code = code;
            return request;
        }

        public static PromoCode FromRPC(this CreatePromoCodeResponse response)
        {
            return response.Code.FromRPC();
        }

        public static PromoCode FromRPC(this GetPromoCodeResponse response)
        {
            return response.Code.FromRPC();
        }

        public static PromoCode FromRPC(this ClaimPromoCodeResponse response)
        {
            return response.Code.FromRPC();
        }

        public static PromoCode FromRPC(this SIPromoCode promoCode)
        {
            var result = new PromoCode();
            result.Code = promoCode.Code;
            result.Creator = promoCode.Creator.FromRPCModel();
            result.Properties = promoCode.Properties;
            result.StartDate = promoCode.ValidFrom;
            result.EndDate = promoCode.ValidUntil;
            result.IsClaimable = promoCode.Claimable;
            result.IsEnabled = promoCode.Enabled;
            result.ClaimCount = (uint)promoCode.NumClaims;
            result.MaxClaimCount = (uint)promoCode.MaxClaims;

            return result;
        }

        #endregion
    }
}
#endif
