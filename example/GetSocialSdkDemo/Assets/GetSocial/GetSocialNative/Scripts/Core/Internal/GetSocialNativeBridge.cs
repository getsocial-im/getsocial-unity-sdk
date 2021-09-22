#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Thrift.Collections;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public class GetSocialNativeBridge : IGetSocialBridge
    {
        private readonly GetSocialStateController _stateController = new GetSocialStateController();

        private string SessionId { get { return _stateController.SessionId; } }
        private bool _initCalled;

        public void Init(string appId)
        {
            if (_initCalled)
            {
                GetSocialLogs.I("GetSocial.Init has been already invoked.");
                return;
            }
            _initCalled = true;
            
            appId = appId ?? _stateController.LoadAppIdFromMetaData();
            var credentials = _stateController.LoadUserCredentials(appId);
            
            Init(new THSdkAuthRequest 
            {
                AppId = appId,
                UserId = credentials.Id,
                Password = credentials.Password,
                SessionProperties = _stateController.SuperProperties
            });
        }
        public void Init(Identity identity, Action onSuccess, Action<GetSocialError> onError)
        {
            if (IsInitialized())
            {
                onError(new GetSocialError(ErrorCodes.IllegalState, "Can not call init with identity when SDK is initialized. Call GetSocial.resetUserWithoutInit first."));
                return;
            }

            _initCalled = true;
            
            var appId = _stateController.LoadAppIdFromMetaData();
            var request = new THSdkAuthRequest 
            {
                AppId = appId,
                SessionProperties = _stateController.SuperProperties,
                Identity = identity.ToRpcModel()
            };
            LogRequest("authenticateSdk", request);
            WithHadesClient(client =>
            {
                return client.authenticateSdkAllInOne(new THSdkAuthRequestAllInOne 
                {
                    SdkAuthRequest = request,
                    ProcessAppOpenRequest = new THProcessAppOpenRequest
                    {}
                });
            }, response =>
            {
                LogResponse("authenticateSdk", response);
                _stateController.Initialized(response, request.AppId);
#if !UNITY_EDITOR
                Track(new AnalyticsEvent() {
                    Name = AnalyticsEventDetails.AppSessionStart,
                    CreatedAt = ApplicationStateListener.Instance.AppStartTime
                });
#endif
                onSuccess();
            }, onError, requireInitialization: false);
        }

        public void Handle(GetSocialAction action)
        {
            // todo
        }

        public bool IsTestDevice()
        {
            return false;
        }

        public string GetDeviceId()
        {
            return _stateController.SuperProperties.DeviceIdfa;
        }

        public string AddOnCurrentUserChangedListener(OnCurrentUserChangedListener listener)
        {
            return _stateController.AddOnUserChangesListener(listener);
        }

        public void RemoveOnCurrentUserChangedListener(string listenerId)
        {
            _stateController.RemoveOnUserChangedListener(listenerId);
        }

        public CurrentUser GetCurrentUser()
        {
            return _stateController.User;
        }

        public void SwitchUser(Identity identity, Action success, Action<GetSocialError> failure)
        {
            var rpcIdentity = identity.ToRpcModel();
            var request = new THSdkAuthRequest
            {
                AppId = _stateController.AppId,
                SessionProperties = _stateController.SuperProperties
            };
            WithHadesClient(client =>
            {
                LogRequest("getPrivateUserByIdentity", rpcIdentity);
                var result = client.getPrivateUserByIdentity(SessionId, rpcIdentity);
                LogResponse("getPrivateUserByIdentity", result);
                request.UserId = result.Id;
                request.Password = result.Password;
                LogRequest("authenticateSdk", request);
                var response = client.authenticateSdk(request);
                LogResponse("authenticateSdk", response);
                Ui(() => _stateController.SaveSession(response.SessionId, response.User, response.UploadEndpoint));
            }, success, failure);
        }

        public void AddIdentity(Identity identity, Action success, Action<ConflictUser> conflict, Action<GetSocialError> failure)
        {
            var rpcIdentity = identity.ToRpcModel();
            WithHadesClient(client =>
            {
                LogRequest("addIdentity", rpcIdentity);
                var result = client.addIdentity(SessionId, rpcIdentity);
                LogResponse("addIdentity", result);
                _stateController.User = result.ToCurrentUser();
            }, success, error =>
            {
                if (error.ErrorCode == (int)THErrorCode.IdentityAlreadyExists || error.ErrorCode == (int) THErrorCode.EMResourceAlreadyExists)
                {
                    WithHadesClient(client =>
                    {
                        LogRequest("getPrivateUserByIdentity", rpcIdentity);
                        var result = client.getPrivateUserByIdentity(SessionId, rpcIdentity);
                        LogResponse("getPrivateUserByIdentity", result);
                        return result.ToConflictUser();
                    }, conflict, failure);
                }
                else
                {
                    failure(error);
                }
            });
        }

        public void RemoveIdentity(string providerId, Action callback, Action<GetSocialError> failure)
        {
            WithHadesClient(client =>
            {
                var request = new THIdentity {Provider = providerId};
                LogRequest("removeIdentity", request);
                var response = client.removeIdentity(SessionId, request);
                LogResponse("removeIdentity", response);
                _stateController.User = response.ToCurrentUser();
            }, callback, failure);
        }

        public void ResetUser(Action success, Action<GetSocialError> failure)
        {
            var request = new THSdkAuthRequest
            {
                AppId = _stateController.AppId,
                SessionProperties = _stateController.SuperProperties
            };
            WithHadesClient(client =>
            {
                LogRequest("authenticateSdk", request);
                var response = client.authenticateSdk(request);
                LogResponse("authenticateSdk", response);
                Ui(() => _stateController.SaveSession(response.SessionId, response.User, response.UploadEndpoint));
            }, success, failure);
        }
        
        public void ResetUserWithoutInit(Action success, Action<GetSocialError> failure)
        {
            _stateController.Uninitialize();
            success();
        }

        public void UpdateDetails(UserUpdate userUpdate, Action callback, Action<GetSocialError> failure)
        {
            WithHadesClient(client =>
            {
                if (userUpdate._avatar64 != null)
                {
                    var b64_bytes = Convert.FromBase64String(userUpdate._avatar64);
                    userUpdate.AvatarUrl = new MediaUploader(_stateController).UploadMedia(b64_bytes, "USER_AVATAR");
                }
                var user = userUpdate.ToRPC();
                LogRequest("updateUser", user);
                var result = client.updateUser(SessionId, user);
                LogResponse("updateUser", result);
                _stateController.User = result.ToCurrentUser();
            }, callback, failure);
        }

        public void Refresh(Action callback, Action<GetSocialError> failure)
        {
            WithHadesClient(client =>
            {
                LogRequest("refresh");
                var result = client.getPrivateUser(SessionId, _stateController.User.Id);
                LogResponse("refresh", result);
                _stateController.User = result.ToCurrentUser();
            }, callback, failure);
        }

        private void Init(THSdkAuthRequest request)
        {
            LogRequest("authenticateSdk", request);
            WithHadesClient(client =>
            {
                while (true)
                {
                    try
                    {
                        try {
                            return client.authenticateSdkAllInOne(new THSdkAuthRequestAllInOne 
                            {
                                SdkAuthRequest = request,
                                ProcessAppOpenRequest = new THProcessAppOpenRequest
                                {

                                }
                            });
                        }
                        catch (THErrors errors)
                        {
                            GetSocialLogs.W(errors.Errors.First().ErrorMsg);
                            throw errors; // if GetSocial exception - rethrow
                        }
                    }
                    catch (Exception exception)
                    {
                        // if system exception - try again
                        Ui(() => GetSocialLogs.W("Failed to init GetSocial, retry in 1 second, exception: " + exception));
                        
                        // wait for 1 sec
                        Thread.Sleep(1000);
                        
                        // unity doesn't stop background threads, so we have to check a state of app
                        if (!EngineUtils.IsApplicationRunning())
                        {
                            return null;
                        }
                    }
                }
            }, response =>
            {
                if (response == null)
                {
                    return;
                } 
                LogResponse("authenticateSdk", response);
                _stateController.Initialized(response, request.AppId);
#if !UNITY_EDITOR
                Track(new AnalyticsEvent() {
                    Name = AnalyticsEventDetails.AppSessionStart,
                    CreatedAt = ApplicationStateListener.Instance.AppStartTime
                });
#endif
            }, requireInitialization: false);
        }

        public void AddOnInitializedListener(Action action)
        {
            if (IsInitialized())
            {
                action.SafeCall();
            }
            else
            {
                _stateController.OnInitializeListeners.Push(action);
            }
        }

        public bool SetLanguage(string language)
        {
            _stateController.SdkLanguage = language;
            UpdateSession();
            return true;
        }

        public string GetNativeSdkVersion()
        {
            return NativeBuildConfig.SdkVersion;
        }

        public bool IsInitialized()
        {
            return _stateController.IsInitialized;
        }

        public string GetLanguage()
        {
            return _stateController.SdkLanguage;
        }

        public void GetAnnouncements(AnnouncementsQuery query, Action<List<Activity>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getAnnouncements", "query = " + query);
            if (query == null)
            {
                onFailure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient(client =>
            {
                var request = query.ToRpc();
                request.SessionId = SessionId;
                var response = client.getAnnouncements(request);
                LogResponse("getAnnouncements", response);
                return response.Data.ConvertAll(it => it.Activity.FromRPCModel());
            }, onSuccess, onFailure);
        }

        public void GetActivities(PagingQuery<ActivitiesQuery> query, Action<PagingResult<Activity>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getActivities", "query = " + query);
            if (query == null || query.Query == null)
            {
                onFailure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient(client =>
            {
                var request = query.ToRpc();
                request.SessionId = SessionId;
                var response = client.getActivitiesV2(request);
                LogResponse("getActivities", response);
                return response.FromRPCModel();
            }, onSuccess, onFailure);
        }

        public void GetActivity(string id, Action<Activity> onSuccess, Action<GetSocialError> onFailure)
        {
            WithHadesClient(client =>
            {
                var request = new GetActivityByIDRequest {ActivityId = id, SessionId = SessionId};
                var response = client.getActivityByID(request);
                LogResponse("getActivityByID", response);
                return response.Activity.FromRPCModel();
            }, onSuccess, onFailure);
        }

        public void PostActivity(ActivityContent content, PostActivityTarget target, Action<Activity> onSuccess, Action<GetSocialError> onFailure)
        {
            WithHadesClient(client =>
            {
                if (content.Attachments.Count != 0)
                {
                    new MediaUploader(_stateController).UploadMedia(content.Attachments, "ACTIVITY_FEED");
                }
                var request = content.ToRpc(_stateController.SdkLanguage, target);
                LogRequest("createActivity");
                request.SessionId = SessionId;
                var response = client.createActivity(request);
                LogResponse("createActivity", response);
                return response.Activity.FromRPCModel();
            }, onSuccess, onFailure);
        }

        public void UpdateActivity(string id, ActivityContent content, Action<Activity> onSuccess, Action<GetSocialError> onFailure)
        {
            WithHadesClient(client =>
            {
                if (content.Attachments.Count != 0)
                {
                    new MediaUploader(_stateController).UploadMedia(content.Attachments, "ACTIVITY_FEED");
                }
                var request = content.ToUpdateRpc(_stateController.SdkLanguage, id);
                LogRequest("updateActivity");
                request.SessionId = SessionId;
                var response = client.updateActivity(request);
                LogResponse("updateActivity", response);
                return response.Activity.FromRPCModel();
            }, onSuccess, onFailure);
        }

        public void RemoveActivities(RemoveActivitiesQuery query, Action onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("removeActivities", "query = " + query);
            if (query == null)
            {
                onFailure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient(client =>
            {
                var request = new DeleteActivitiesRequest();
                request.SessionId = SessionId;
                request.Ids = new THashSet<string>();
                request.Ids.AddAll(query.Ids);
                var response = client.deleteActivities(request);
                LogResponse("removeActivities", response);
            }, onSuccess, onFailure);
        }


        public void AddReaction(string reaction, string activityId, Action success, Action<GetSocialError> failure)
        {
            WithHadesClient(client =>
            {
                var request = new CreateReactionRequest();
                request.SessionId = SessionId;
                request.Reaction = reaction;
                request.Id = new SGEntity();
                request.Id.Id = activityId;
                request.Id.EntityType = (int) SGEntityType.ACTIVITY;
                LogRequest("createReaction", request);
                var response = client.createReaction(request);
                LogResponse("createReaction", response);
            }, success, failure);
        }

        public void RemoveReaction(string reaction, string activityId, Action success, Action<GetSocialError> failure)
        {
            WithHadesClient(client =>
            {
                var request = new DeleteReactionRequest();
                request.SessionId = SessionId;
                request.Reaction = reaction;
                request.Id = new SGEntity();
                request.Id.Id = activityId;
                request.Id.EntityType = (int) SGEntityType.ACTIVITY;
                LogRequest("deleteReaction", request);
                var response = client.deleteReaction(request);
                LogResponse("deleteReaction", response);
            }, success, failure);
        }

        public void AddVotes(HashSet<string> pollOptionIds, string activityId, Action onSuccess, Action<GetSocialError> onError)
        {
            WithHadesClient(client =>
            {
                var request = new CreateVoteRequest();
                request.SessionId = SessionId;
                request.OptionIds = new THashSet<string>();
                request.OptionIds.AddAll(pollOptionIds);
                request.KeepExisting = true;
                request.Target = new SGEntity();
                request.Target.Id = activityId;
                request.Target.EntityType = (int)SGEntityType.ACTIVITY;
                LogRequest("addVotes", request);
                var response = client.createVote(request);
                LogResponse("addVotes", response);
            }, onSuccess, onError);
        }

        public void SetVotes(HashSet<string> pollOptionIds, string activityId, Action onSuccess, Action<GetSocialError> onError)
        {
            WithHadesClient(client =>
            {
                var request = new CreateVoteRequest();
                request.SessionId = SessionId;
                request.OptionIds = new THashSet<string>();
                request.OptionIds.AddAll(pollOptionIds);
                request.KeepExisting = false;
                request.Target = new SGEntity();
                request.Target.Id = activityId;
                request.Target.EntityType = (int)SGEntityType.ACTIVITY;
                LogRequest("setVotes", request);
                var response = client.createVote(request);
                LogResponse("setVotes", response);
            }, onSuccess, onError);
        }

        public void RemoveVotes(HashSet<string> pollOptionIds, string activityId, Action onSuccess, Action<GetSocialError> onError)
        {
            WithHadesClient(client =>
            {
                var request = new DeleteVoteRequest();
                request.SessionId = SessionId;
                request.OptionIds = new THashSet<string>();
                request.OptionIds.AddAll(pollOptionIds);
                request.Target = new SGEntity();
                request.Target.Id = activityId;
                request.Target.EntityType = (int)SGEntityType.ACTIVITY;
                LogRequest("removeVotes", request);
                var response = client.deleteVote(request);
                LogResponse("removeVotes", response);
            }, onSuccess, onError);
        }

        public void GetVotes(PagingQuery<VotesQuery> query, Action<PagingResult<UserVotes>> onSuccess, Action<GetSocialError> onError)
        {
            if (query == null || query.Query == null)
            {
                onError(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient(client =>
            {
                var request = query.ToRpc();
                request.SessionId = SessionId;
                var response = client.getVotes(request);
                LogResponse("getVotes", response);
                return response.FromRPCModel();
            }, onSuccess, onError);
        }

        public void GetTags(TagsQuery query, Action<List<string>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("findTagsV2", "query = " + query);
            if (query == null)
            {
                onFailure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient(client =>
            {
                var request = query.ToRpc();
                request.SessionId = SessionId;
                var response = client.findTagsV2(request);
                LogResponse("findTagsV2", response);
                return response.Tags ?? new List<string>();
            }, onSuccess, onFailure);
        }

        public void ReportActivity(string activityId, ReportingReason reason, string explanation, Action onSuccess, Action<GetSocialError> onError)
        {
            WithHadesClient(client =>
            {
                var request = new ReportEntityV2Request 
                {
                    Id = new SGEntity { EntityType = (int) SGEntityType.ACTIVITY, Id = activityId }, 
                    Explanation = explanation, 
                    Reason = (THReportingReason) reason
                };
                LogRequest("reportEntityV2", request);
                request.SessionId = SessionId;
                var response = client.reportEntityV2(request);
                LogResponse("reportEntityV2", response);
            }, onSuccess, onError);
        }
        public void GetReactions(PagingQuery<ReactionsQuery> query, Action<PagingResult<UserReactions>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getReactions", "query = " + query);
            if (query == null || query.Query == null)
            {
                onFailure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient(client =>
            {
                var request = query.ToRpc();
                request.SessionId = SessionId;
                var response = client.getReactions(request);
                LogResponse("getReactions", response);
                return response.FromRPCModel();
            }, onSuccess, onFailure);
        }

        public void GetUsers(PagingQuery<UsersQuery> query, Action<PagingResult<User>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getUsers", "query = " + query);
            if (query == null || query.Query == null)
            {
                onFailure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient(client =>
            {
                var request = query.ToRpc();
                request.SessionId = SessionId;
                var response = client.getUsers(request);
                LogResponse("getUsers", response);
                return response.FromRPCModel();
            }, onSuccess, onFailure);
        }

        public void GetUsers(UserIdList list, Action<Dictionary<string, User>> onSuccess, Action<GetSocialError> onFailure)
        {
            WithHadesClient(client =>
            {
                var request = new GetUsersRequestById { UserIds = list.AsString()};
                LogRequest("getUsers", request);
                request.SessionId = SessionId;
                var response = client.getUsersById(request);
                LogResponse("getUsers", response);
                return response.Users.ToDictionary(pair => pair.Key, pair => pair.Value.FromRPCModel());
            }, onSuccess, onFailure);        }

        public void GetUsersCount(UsersQuery query, Action<int> success, Action<GetSocialError> error)
        {
            LogRequest("getUsers", "query = " + query);
            WithHadesClient(client =>
            {
                var request = query.ToRpc();
                request.Pagination = new Pagination {Limit = 1};
                request.SessionId = SessionId;
                var response = client.getUsers(request);
                LogResponse("getUsers", response);
                return response.TotalNumber;
            }, success, error);
        }

        public void AddFriends(UserIdList userIds, Action<int> success, Action<GetSocialError> failure)
        {
            WithHadesClient(client =>
            {
                var request = new AddFriendsRequest {UserIds = new THashSet<string>()};
                request.UserIds.AddAll(userIds.AsString());
                LogRequest("addFriends", request);
                request.SessionId = SessionId;
                var response = client.addFriends(request);
                LogResponse("addFriends", response);
                return response.NumberOfFriends;
            }, success, failure);        }

        public void RemoveFriends(UserIdList userIds, Action<int> success, Action<GetSocialError> failure)
        {
            WithHadesClient(client =>
            {
                var request = new RemoveFriendsRequest {UserIds = new THashSet<string>()};
                request.UserIds.AddAll(userIds.AsString());
                LogRequest("removeFriends", request);
                request.SessionId = SessionId;
                var response = client.removeFriends(request);
                LogResponse("removeFriends", response);
                return response.NumberOfFriends;
            }, success, failure);        }

        public void GetFriends(PagingQuery<FriendsQuery> query, Action<PagingResult<User>> success, Action<GetSocialError> failure)
        {
            LogRequest("getFriends", "query = " + query);
            if (query == null || query.Query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient(client =>
            {
                var request = query.ToRpc();
                request.SessionId = SessionId;
                var response = client.getFriendsV2(request);
                LogResponse("getFriends", response);
                return response.FromRPCModel();
            }, success, failure);
        }

        public void GetFriendsCount(FriendsQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            LogRequest("getFriends", "query = " + query);
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient(client =>
            {
                var request = query.ToRpc();
                request.SessionId = SessionId;
                var response = client.getFriendsV2(request);
                LogResponse("getFriends", response);
                return response.TotalNumber;
            }, success, failure);
        }

        public void AreFriends(UserIdList userIds, Action<Dictionary<string, bool>> success, Action<GetSocialError> failure)
        {
            WithHadesClient(client =>
            {
                var request = new AreFriendsRequest {UserIds = new THashSet<string>()};
                request.UserIds.AddAll(userIds.AsString());
                LogRequest("areFriends", request);
                request.SessionId = SessionId;
                var response = client.areFriends(request);
                LogResponse("areFriends", response);
                return response.FromRPCModel(request.UserIds);
            }, success, failure);
        }

        public void IsFriend(UserId userId, Action<bool> success, Action<GetSocialError> failure)
        {
            AreFriends(UserIdList.CreateWithProvider(userId.Provider, userId.Id), result =>
            {
                if (result.ContainsKey(userId.Id))
                {
                    success(result[userId.Id]);
                }
                else
                {
                    failure(new GetSocialError(ErrorCodes.IllegalArgument, $"User {userId} does not exist."));
                }
            }, failure);
        }

        public void SetFriends(UserIdList userIds, Action<int> success, Action<GetSocialError> failure)
        {
            WithHadesClient(client =>
            {
                if (userIds.ProviderId == null)
                {
                    return client.setFriends(SessionId, userIds.UserIds);
                }
                else
                {
                    return client.setFriendsByIdentity(SessionId, userIds.ProviderId, userIds.UserIds);
                }
            }, success, failure);
        }

        public void GetSuggestedFriends(SimplePagingQuery query, Action<PagingResult<SuggestedFriend>> success, Action<GetSocialError> failure)
        {
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient(client =>
            {
                var request = new GetSuggestedFriendsRequest
                {
                    Pagination = query.ToPagination(),
                    SessionId = SessionId
                };
                LogRequest("getSuggestedFriendsV2", request);

                var response = client.getSuggestedFriendsV2(request);
                LogResponse("getSuggestedFriendsV2", response);
                
                return response.FromRPCModel();
            }, success, failure);
        }
        #region Topics

        public void GetTopic(string topic, Action<Topic> success, Action<GetSocialError> failure)
        {
            LogRequest("getTopic", "topicId = " + topic);
            WithHadesClient((client) => {
                var request = new GetTopicRequest();
                request.Id = topic;
                request.SessionId = SessionId;
                var response = client.getTopic(request);
                LogResponse("getTopic", response);
                return (response.Topic.FromRPCModel());
            }, success, failure);
        }

        public void GetTopics(PagingQuery<TopicsQuery> pagingQuery, Action<PagingResult<Topic>> success, Action<GetSocialError> failure)
        {
            LogRequest("getTopics", "query = " + pagingQuery);
            if (pagingQuery == null || pagingQuery.Query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = pagingQuery.ToRPC();
                request.SessionId = SessionId;
                var response = client.getTopics(request);
                LogResponse("getTopics", response);
                return (response.FromRPCModel());
            }, success, failure);
        }

        public void GetTopicsCount(TopicsQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            LogRequest("getTopics", "query = " + query);
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.ToRPC();
                request.SessionId = SessionId;
                var response = client.getTopics(request);
                LogResponse("getTopics", response);
                return (response.TotalNumber);
            }, success, failure);
        }

        #endregion

        #region Groups

        public void CreateGroup(GroupContent content, Action<Group> success, Action<GetSocialError> failure)
        {
            LogRequest("createGroup", "content = " + content);
            if (content.Avatar != null && content.Avatar.Image != null)
            {
                var attachmentList = new List<MediaAttachment>();
                attachmentList.Add(content.Avatar);
                new MediaUploader(_stateController).UploadMedia(attachmentList, "USER_AVATAR");
            }
            WithHadesClient((client) => {
                var request = content.ToRPC(_stateController.SdkLanguage);
                request.SessionId = SessionId;
                var response = client.createGroup(request);
                LogResponse("createGroup", response);
                return (response.Group.FromRPCModel());
            }, success, failure);
        }

        public void UpdateGroup(string groupId, GroupContent content, Action<Group> success, Action<GetSocialError> failure)
        {
            LogRequest("updateGroup", "groupId = " + groupId + ", content = " + content);
            WithHadesClient((client) => {
                var request = content.ToUpdateRPC(_stateController.SdkLanguage);
                request.SessionId = SessionId;
                request.Id = groupId;
                var response = client.updateGroup(request);
                LogResponse("updateGroup", response);
                return (response.Group.FromRPCModel());
            }, success, failure);
        }

        public void RemoveGroups(List<string> groupIds, Action success, Action<GetSocialError> failure)
        {
            LogRequest("removeGroups", "groupIds = " + groupIds);
            WithHadesClient((client) => {
                var request = new DeleteGroupsRequest();
                request.Ids = groupIds;
                request.SessionId = SessionId;
                var response = client.deleteGroups(request);
                LogResponse("removeGroups", response);
            }, success, failure);
        }

        public void GetGroupMembers(PagingQuery<MembersQuery> pagingQuery, Action<PagingResult<GroupMember>> success, Action<GetSocialError> failure)
        {
            LogRequest("getMembers", "query = " + pagingQuery);
            if (pagingQuery == null || pagingQuery.Query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = pagingQuery.ToRPC();
                request.SessionId = SessionId;
                var response = client.getGroupMembers(request);
                LogResponse("getMembers", response);
                return (response.FromRPCModel());
            }, success, failure);
        }

        public void GetGroups(PagingQuery<GroupsQuery> pagingQuery, Action<PagingResult<Group>> success, Action<GetSocialError> failure)
        {
            LogRequest("getGroups", "query = " + pagingQuery);
            if (pagingQuery == null || pagingQuery.Query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = pagingQuery.ToRPC();
                request.SessionId = SessionId;
                var response = client.getGroups(request);
                LogResponse("getGroups", response);
                return (response.FromRPCModel());
            }, success, failure);
        }

        public void GetGroupsCount(GroupsQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            LogRequest("getGroups", "query = " + query);
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.ToRPC();
                request.SessionId = SessionId;
                var response = client.getGroups(request);
                LogResponse("getGroups", response);
                return (response.TotalNumber);
            }, success, failure);
        }

        public void GetGroup(string groupId, Action<Group> success, Action<GetSocialError> failure)
        {
            LogRequest("getGroup", "groupId = " + groupId);
            WithHadesClient((client) => {
                var request = new GetGroupRequest();
                request.Id = groupId;
                request.SessionId = SessionId;
                var response = client.getGroup(request);
                LogResponse("getGroup", response);
                return (response.Group.FromRPCModel());
            }, success, failure);
        }

        public void AddGroupMembers(AddGroupMembersQuery query, Action<List<GroupMember>> success, Action<GetSocialError> failure)
        {
            LogRequest("addGroupMembers", "query = " + query);
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.InternalQuery.ToRPC();
                request.SessionId = SessionId;
                var response = client.updateGroupMembers(request);
                LogResponse("addGroupMembers", response);
                return (response.Members.ConvertAll(sgMember => sgMember.FromRPCModel()));
            }, success, failure);
        }

        public void JoinGroup(JoinGroupQuery query, Action<GroupMember> success, Action<GetSocialError> failure)
        {
            LogRequest("joinGroup", "query = " + query);
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.InternalQuery.ToRPC();
                request.SessionId = SessionId;
                var response = client.updateGroupMembers(request);
                LogResponse("joinGroup", response);
                return (response.Members.ConvertAll(sgMember => sgMember.FromRPCModel()).First());
            }, success, failure);
        }

        public void UpdateGroupMembers(UpdateGroupMembersQuery query, Action<List<GroupMember>> success, Action<GetSocialError> failure)
        {
            LogRequest("updateGroupMembers", "query = " + query);
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.ToRPC();
                request.SessionId = SessionId;
                var response = client.updateGroupMembers(request);
                LogResponse("updateGroupMembers", response);
                return (response.Members.ConvertAll(sgMember => sgMember.FromRPCModel()));
            }, success, failure);
        }

        public void RemoveGroupMembers(RemoveGroupMembersQuery query, Action success, Action<GetSocialError> failure)
        {
            LogRequest("removeGroupMembers", "query = " + query);
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.ToRPC();
                request.SessionId = SessionId;
                var response = client.removeGroupMembers(request);
                LogResponse("removeGroupMembers", response);
            }, success, failure);
        }

        public void AreGroupMembers(string groupId, UserIdList userIdList, Action<Dictionary<string, Membership>> success, Action<GetSocialError> failure)
        {
            LogRequest("areMembers", "groupId = " + groupId + " userIdList = " + userIdList);
            WithHadesClient((client) => {
                var request = new AreGroupMembersRequest();
                request.GroupId = groupId;
                request.UserIds = userIdList.AsString();
                request.SessionId = SessionId;
                var response = client.areGroupMembers(request);
                LogResponse("areMembers", response);
                return (response.FromRPCModel());
            }, success, failure);
        }

        #endregion

        #region Chats
        public void SendChatMessage(ChatMessageContent content, ChatId target, Action<ChatMessage> success, Action<GetSocialError> failure)
        {
            LogRequest("sendChatMessage", "content = " + content + ", target = " + target);
            WithHadesClient((client) => {
                var request = new SendChatMessageRequest();
                request.SessionId = SessionId;
                request.Content = content.ToRPCModel();
                request.Id = target.Id;
                request.UserId = target.UserId.ToRpc();
                var response = client.sendChatMessage(request);
                LogResponse("sendChatMessage", response);
                return (response.Message.FromRPCModel());
            }, success, failure);
        }

        public void GetChatMessages(ChatMessagesPagingQuery pagingQuery, Action<ChatMessagesPagingResult> success, Action<GetSocialError> failure)
        {
            LogRequest("getChatMessages", "pagingQuery = " + pagingQuery);
            WithHadesClient((client) => {
                var request = new GetChatMessagesRequest();
                request.SessionId = SessionId;
                request.Pagination = pagingQuery.ToRPCModel();
                request.Id = pagingQuery.Query.Id.Id;
                request.UserId = pagingQuery.Query.Id.UserId.ToRpc();
                var response = client.getChatMessages(request);
                LogResponse("getChatMessages", response);
                return (response.FromRPCModel());
            }, success, failure);
        }

        public void GetChats(SimplePagingQuery pagingQuery, Action<PagingResult<Chat>> success, Action<GetSocialError> failure)
        {
            LogRequest("getChats", "pagingQuery = " + pagingQuery);
            WithHadesClient((client) => {
                var request = new GetChatsRequest();
                request.SessionId = SessionId;
                request.Pagination = pagingQuery.ToPagination();
                var response = client.getChats(request);
                LogResponse("getChats", response);
                return (response.FromRPCModel());
            }, success, failure);
        }

        public void GetChat(ChatId chatId, Action<Chat> success, Action<GetSocialError> failure)
        {
            LogRequest("getChat", "chatId = " + chatId);
            WithHadesClient((client) => {
                var request = new GetChatRequest();
                request.SessionId = SessionId;
                request.Id = chatId.Id;
                request.UserId = chatId.UserId.ToRpc();
                var response = client.getChat(request);
                LogResponse("getChat", response);
                return response.FromRPCModel();
            }, success, failure);
        }
        #endregion

        #region Followers
        public void Follow(FollowQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            LogRequest("followEntities", "query = " + query);
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.ToRPC();
                request.SessionId = SessionId;
                var response = client.followEntities(request);
                LogResponse("followEntities", response);
                return (response.TotalFollowedEntitiesCount);
            }, success, failure);
        }

        public void Unfollow(FollowQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            LogRequest("unfollowEntities", "query = " + query);
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.ToUnfollowRPC();
                request.SessionId = SessionId;
                var response = client.unfollowEntities(request);
                LogResponse("unfollowEntities", response);
                return (response.TotalFollowedEntitiesCount);
            }, success, failure);
        }

        public void IsFollowing(UserId userId, FollowQuery query, Action<Dictionary<string, bool>> success, Action<GetSocialError> failure)
        {
            LogRequest("isFollowing", "query = " + query);
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.ToIsFollowingRPC();
                request.UserId = userId.AsString();
                LogRequest("isFollowing", request);
                request.SessionId = SessionId;
                var response = client.isFollowing(request);
                LogResponse("isFollowing", response);
                return response.FromRPCModel(request.EntityIds);
            }, success, failure);
        }

        public void GetFollowers(PagingQuery<FollowersQuery> query, Action<PagingResult<User>> success, Action<GetSocialError> failure)
        {
            LogRequest("getEntityFollowers", "query = " + query);
            if (query == null || query.Query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.ToRPC();
                request.SessionId = SessionId;
                var response = client.getEntityFollowers(request);
                LogResponse("getEntityFollowers", response);
                return (response.FromRPCModel());
            }, success, failure);
        }

        public void GetFollowersCount(FollowersQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            LogRequest("getEntityFollowers", "query = " + query);
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.ToRPC();
                request.SessionId = SessionId;
                var response = client.getEntityFollowers(request);
                LogResponse("getEntityFollowers", response);
                return (response.TotalNumber);
            }, success, failure);
        }

        #endregion

        #region Invites

        public void GetAvailableChannels(Action<List<InviteChannel>> success, Action<GetSocialError> failure)
        {
            if (IsInitialized())
            {
                success(_stateController.InviteChannels.ConvertAll(it => it.FromRpcModel(_stateController.SdkLanguage)));
            }
            else
            {
                failure(new GetSocialError(ErrorCodes.IllegalState, "SDK is not initialized"));
            }
        }

        public void Send(InviteContent customInviteContent, string channelId, Action success, Action cancel, Action<GetSocialError> failure)
        {
            Debug.Log("Invites are not supported");
        }

        public void Create(InviteContent customInviteContent, Action<Invite> success, Action<GetSocialError> failure)
        {
            Debug.Log("Invites are not supported");
        }

        public void CreateLink(Dictionary<string, object> linkParams, Action<string> success, Action<GetSocialError> failure)
        {
            WithHadesClient(client =>
            {
                var request = new THCreateTokenRequest
                {
                    ProviderId = "manual", 
                    LinkParams = linkParams.ToDictionaryOfStrings() 
                };
                LogRequest("createInviteUrl", request);

                var response = client.createInviteUrl(SessionId, request);
                LogResponse("createInviteUrl", response);

                return response.Url;
            }, success, failure);
        }

        public bool RegisterPlugin(string channelId, object plugin)
        {
            Debug.Log("Invites are not supported");
            return false;
        }

        public void GetReferredUsers(PagingQuery<ReferralUsersQuery> query, Action<PagingResult<ReferralUser>> success, Action<GetSocialError> failure)
        {
            LogRequest("getReferredUsersV2", query);
            if (query == null || query.Query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var offset = SafeParse(query.NextCursor);
                var response = client.getReferredUsersV2(SessionId, query.Query.GetEventName(), offset, query.Limit);
                LogResponse("getReferredUsersV2", response);
                var entries = response.ConvertAll(it => it.FromRPCModel());
                return ThriftConversions.ToPagingResult(entries, offset, query.Limit);
            }, success, failure);
        }

        private static int SafeParse(string val)
        {
            int res;
            if (int.TryParse(val, out res))
            {
                return res;
            }

            return 0;
        }

        public void GetReferrerUsers(PagingQuery<ReferralUsersQuery> query, Action<PagingResult<ReferralUser>> success, Action<GetSocialError> failure)
        {
            LogRequest("getReferrerUsers", query);
            if (query == null || query.Query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var offset = SafeParse(query.NextCursor);
                var response = client.getReferrerUsers(SessionId, query.Query.GetEventName(), offset, query.Limit);
                LogResponse("getReferrerUsers", response);
                var entries = response.ConvertAll(it => it.FromRPCModel());
                return ThriftConversions.ToPagingResult(entries, offset, query.Limit);
            }, success, failure);
        }

        public void SetReferrer(UserId userId, string eventName, Dictionary<string, string> customData, Action success, Action<GetSocialError> failure)
        {
            WithHadesClient((client) => {
                LogRequest("setReferrer", $"userId={userId}, event={eventName}, data={customData.ToDebugString()}");
                var response = client.setReferrer(SessionId, userId.AsString(), eventName, customData);
                LogResponse("setReferrer", response);
            }, success, failure);
        }

        public void SetOnReferralDataReceivedListener(Action<ReferralData> action)
        {
            _stateController.ReferralDataListener = action;
            Debug.Log("To test referral data listener method in Unity Editor, call ");
        }

        #endregion

        #region Promo Codes

        public void CreatePromoCode(PromoCodeContent content, Action<PromoCode> success, Action<GetSocialError> failure)
        {
            LogRequest("createPromoCode", "content = " + content);
            WithHadesClient((client) => {
                var request = content.ToRPC();
                request.SessionId = SessionId;
                var response = client.createPromoCode(request);
                LogResponse("createPromoCode", response);
                return (response.FromRPC());
            }, success, failure);
        }

        public void GetPromoCode(string code, Action<PromoCode> success, Action<GetSocialError> failure)
        {
            LogRequest("getPromoCode", "code = " + code);
            WithHadesClient((client) => {
                var request = new GetPromoCodeRequest();
                request.Code = code;
                request.SessionId = SessionId;
                var response = client.getPromoCodeV2(request);
                LogResponse("createPromoCode", response);
                return (response.FromRPC());
            }, success, failure);
        }

        public void ClaimPromoCode(string code, Action<PromoCode> success, Action<GetSocialError> failure)
        {
            LogRequest("claimPromoCode", "code = " + code);
            WithHadesClient((client) => {
                var request = new ClaimPromoCodeRequest();
                request.Code = code;
                request.SessionId = SessionId;
                var response = client.claimPromoCodeV2(request);
                LogResponse("claimPromoCode", response);
                return (response.FromRPC());
            }, success, failure);
        }

        #endregion

        #region Analytics

        public bool TrackPurchase(PurchaseData purchaseData)
        {
            Debug.Log("Analytics are not available");
            return true;
        }

        public bool TrackCustomEvent(string eventName, Dictionary<string, string> eventData)
        {
            Track(new AnalyticsEvent {
                Name = eventName,
                Properties = eventData,
                IsCustom = true
            });
            return true;
        }
        
        internal void Track(AnalyticsEvent e) 
        {
            var properties = _stateController.SuperProperties;
            WithHadesClient((client) => {
                var rpcEvent = e.ShiftTime(_stateController.ServerTimeDiff).ToRpcModel();
                LogRequest("trackAnalyticsEvents", string.Format("event={0}", rpcEvent.ToString()));
                var response = client.trackAnalyticsEvents(SessionId, properties, new List<THAnalyticsBaseEvent> {
                    rpcEvent
                });
                LogResponse("trackAnalyticsEvents", response);
            });
        }

        #endregion

        #region Push notifications

        public void GetNotifications(PagingQuery<NotificationsQuery> query, Action<PagingResult<Notification>> success, Action<GetSocialError> failure)
        {
            LogRequest("getNotifications, query = ", query);
            if (query == null || query.Query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.ToRPC();
                request.SessionId = SessionId;
                var response = client.getNotifications(request);
                LogResponse("getNotifications", response);
                return response.FromRPCModel();
            }, success, failure);
        }

        public void CountNotifications(NotificationsQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            LogRequest("getNotifications, query = ", query);
            if (query == null)
            {
                failure(new GetSocialError(ErrorCodes.IllegalArgument, "query parameter is null"));
                return;
            }
            WithHadesClient((client) => {
                var request = query.ToRPC();
                request.Pagination = new Pagination {Limit = 1};
                request.SessionId = SessionId;
                var response = client.getNotifications(request);
                LogResponse("getNotifications", response);
                return response.TotalNumber;
            }, success, failure);
        }

        public void SetStatus(string newStatus, List<string> notificationIds, Action success, Action<GetSocialError> failure)
        {
            WithHadesClient((client) =>
            {
                var request = new THNotificationsSetStatusParams
                {
                    Ids = notificationIds,
                    Status = newStatus
                };
                LogRequest("setNotificationStatus", request);
                var response = client.setNotificationsStatus(SessionId, request);
                LogResponse("setNotificationStatus", response);
            }, success, failure);
        }

        public void SetPushNotificationsEnabled(bool enabled, Action success, Action<GetSocialError> failure)
        {
            WithHadesClient((client) =>
            {
                LogRequest("setPushNotificationsEnabled", enabled);
                var response = client.setPushNotificationsEnabled(SessionId, enabled);
                LogResponse("setPushNotificationsEnabled", response);
            }, success, failure);
        }

        public void ArePushNotificationsEnabled(Action<bool> success, Action<GetSocialError> failure)
        {
            WithHadesClient(client => client.isPushNotificationsEnabled(SessionId), success, failure);
        }

        public void Send(NotificationContent content, SendNotificationTarget target, Action success, Action<GetSocialError> failure)
        {
            WithHadesClient(client =>
            {
                var userIds = target.UserIdList.AsString();
                userIds.AddAll(target.PlaceholderIds);
                //todo add image
                var customNotification = new SendNotificationRequest
                {
                    UserIds = userIds, 
                    Action = content._action?.ToRpcModel(),
                    Text = content._text,
                    Title = content._title,
                    SessionId = SessionId,
                    ActionButtons = content._actionButtons.ConvertAll(it => it.ToRpcModel()),
                    Badge = content._badge?.ToRpcModel(),
                    TemplateName = content._templateName,
                    TemplateData = content._templatePlaceholders,
                    Media = new THNotificationTemplateMedia
                    {
                        BackgroundImage = content._customization?.BackgroundImageConfiguration,
                        TextColor = content._customization?.TextColor,
                        TitleColor = content._customization?.TitleColor
                    }
                };
                client.sendNotification(customNotification);
            }, success, failure);
        }

        public void RegisterDevice()
        {
            Debug.Log("Push notifications are not available");
        }

        public void SetOnNotificationReceivedListener(Action<Notification> listener)
        {
            Debug.Log("Push notifications are not available");
        }

        public void SetOnNotificationClickedListener(Action<Notification, NotificationContext> listener)
        {
            Debug.Log("Push notifications are not available");
        }

        public void SetOnTokenReceivedListener(Action<string> listener)
        {
            Debug.Log("Push notifications are not available");
        }

        public void HandleOnStartUnityEvent()
        {
            Debug.Log("Unity started.");
        }

        public void TriggerOnReferralDataReceivedListener(ReferralData referralData)
        {
            if (_stateController.ReferralDataListener != null)
            {
                _stateController.ReferralDataListener(referralData);
            }
        }

        #endregion

        private void UpdateSession()
        {
            var superProperties = _stateController.SuperProperties;
            if (IsInitialized())
            {
                LogRequest("updateSession", superProperties);
                WithHadesClient(client =>
                {
                    var response = client.updateSession(SessionId, superProperties);
                    Ui(() => LogResponse("updateSession", response));
                });
            }
        }

        private void WithHadesClient(Action<Hades.Client> whenReady, Action success = null, Action<GetSocialError> onError = null, bool requireInitialization = true)
        {
            if (requireInitialization && !IsInitialized())
            {
                GetSocialLogs.W("Failed to call GetSocial before it is initialized.");
                onError.SafeCall(new GetSocialError(ErrorCodes.IllegalState, "Failed to call GetSocial before it is initialized"));
                return;
            }
            HadesClientFactory.Create(client =>
            {
                Action<GetSocialError> handleError = getSocialError =>
                {
                    Ui(() =>
                    {
                        GetSocialLogs.W("Error: " + getSocialError);
                        onError.SafeCall(getSocialError);
                    });
                };
                try
                {
                    whenReady(client);
                    Ui(() => success.SafeCall());
                } 
                catch (THErrors error)
                {
                    var first = error.Errors.First();
                    handleError(new GetSocialError((int) first.ErrorCode, first.ErrorMsg));
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                    handleError(new GetSocialError(exception.Message));
                }
            });
        }
        
        private void WithHadesClient<T>(Func<Hades.Client, T> whenReady, Action<T> onSuccess, Action<GetSocialError> onError = null, bool requireInitialization = true)
        {
            if (requireInitialization && !IsInitialized())
            {
                GetSocialLogs.W("Failed to call GetSocial before it is initialized.");
                onError.SafeCall(new GetSocialError(ErrorCodes.IllegalState, "Failed to call GetSocial before it is initialized"));
                return;
            }
            HadesClientFactory.Create(client =>
            {
                Action<GetSocialError> handleError = getSocialError =>
                {
                    Ui(() =>
                    {
                        GetSocialLogs.W("Error: " + getSocialError);
                        onError.SafeCall(getSocialError);
                    });
                };
                try
                {
                    var result = whenReady(client);
                    Ui(() => onSuccess(result));
                } 
                catch (THErrors error)
                {
                    var first = error.Errors.First();
                    handleError(new GetSocialError((int) first.ErrorCode, first.ErrorMsg));
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                    handleError(new GetSocialError(exception.Message));
                }
            });
        }

        private static void Ui(Action onUiThread)
        {
            MainThreadExecutor.Queue(onUiThread);
        }

        private static void LogRequest(string method, object parameters = null)
        {
            try
            {
                var log = parameters == null
                    ? string.Format("[Request] {0}", method)
                    : string.Format("[Request] {0}, body: {1}", method, parameters);
                Debug.Log(log);
            }
            catch
            {
                // Just to be sure we won't crash in logs
            }
        }

        private static void LogResponse(string method, object parameters)
        {
            try
            {
                GetSocialLogs.D(string.Format("[Response] {0}: {1}", method, parameters));
            }
            catch
            {
                // Just to be sure we won't crash in logs
            }
        }
    }
}
#endif