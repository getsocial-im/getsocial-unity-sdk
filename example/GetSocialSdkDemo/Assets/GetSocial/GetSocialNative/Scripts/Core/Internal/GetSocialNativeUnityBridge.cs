#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using GetSocialSdk.Core.Analytics;
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal class GetSocialNativeUnityBridge : IGetSocialNativeBridge
    {
        private readonly GetSocialStateController _stateController = new GetSocialStateController();
        
        private string SessionId { get { return _stateController.SessionId; } }
        private THPrivateUser User { 
            get { return _stateController.User; }
            set { _stateController.User = value; }
        }
        
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

        private void Init(THSdkAuthRequest request)
        {
            LogRequest("authenticateSdk", request);
            WithHadesClient(client =>
            {
                THSdkAuthResponseAllInOne response = null;
                while (response == null)
                {
                    try
                    {
                        try {
                            response = client.authenticateSdkAllInOne(new THSdkAuthRequestAllInOne 
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
                            return;
                        }
                    }
                }
                Ui(() =>
                {
                    LogResponse("authenticateSdk", response);
                    _stateController.Initialized(response, request.AppId);
#if !UNITY_EDITOR
                    TrackAnalyticsEvent(new AnalyticsEvent
                    {
                        Name = AnalyticsEventDetails.AppSessionStart,
                        CreatedAt = ApplicationStateListener.Instance.AppStartTime
                    });
#endif
                });
            }, requireInitialization: false);
        }

        public void WhenInitialized(Action action)
        {
            if (IsInitialized)
            {
                action.SafeCall();
            }
            else
            {
                _stateController.OnInit = action;
            }
        }

        public bool IsInitialized
        {
            get { return _stateController.IsInitialized; }
        }

        public string GetNativeSdkVersion()
        {
            return NativeBuildConfig.SdkVersion;
        }

        public string GetLanguage()
        {
            return _stateController.SdkLanguage;
        }

        public bool SetLanguage(string languageCode)
        {
            _stateController.SdkLanguage = languageCode;
            UpdateSession();
            return true;
        }

        public bool SetGlobalErrorListener(Action<GetSocialError> onError)
        {
            _stateController.OnError = onError;
            return true;
        }

        public bool RemoveGlobalErrorListener()
        {
            _stateController.OnError = null;
            return true;
        }

        public bool IsInviteChannelAvailable(string channelId)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), channelId);
            return false;
        }

        public void ProcessReferrer(string referrer, Action<ReferralData> onSuccess, Action<GetSocialError> onError)
        {
            var appOpenRequest = new THProcessAppOpenRequest
            {
                IsNewInstall = _stateController.IsNewInstall,
                ReferrerData = new Dictionary<THAppOpenSource, Dictionary<THAppOpenKey, string>>
                {
                    {THAppOpenSource.Manual, new Dictionary<THAppOpenKey, string>
                    {
                        {THAppOpenKey.Value, referrer}
                    }}
                }
            };
            LogRequest("processAppOpen", "isNewInstall=" + appOpenRequest.IsNewInstall + "; referrer=" + referrer);
            WithHadesClient(client =>
            {
                try
                {
                    var rpcReferralData = client.processAppOpen(SessionId, appOpenRequest);
                    var referralData = rpcReferralData.ToReferralData();
                    Ui(() =>
                    {
                        LogResponse("processAppOpen", rpcReferralData);
                        onSuccess.SafeCall(referralData);
                    });
                }
                catch (THErrors errors)
                {
                    if (errors.Errors.First().ErrorCode == THErrorCode.SIErrProcessAppOpenNoMatch)
                    {
                        Ui(() =>
                        {
                            LogResponse("processAppOpen", null);
                            onSuccess.SafeCall(null);
                        });
                    }
                    else
                    {
                        throw;
                    }
                }
            }, onError);
        }
        static readonly InviteChannel[] EmptyChannels = { };
        public InviteChannel[] InviteChannels
        {
            get
            {
                return IsInitialized 
                    ? _stateController.InviteChannels.ConvertAll(channel => channel.FromRpcModel(GetLanguage())).ToArray() 
                    : EmptyChannels;
            }
        }

        public void SendInvite(string channelId, Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
            SendInvite(channelId, null, onComplete, onCancel, onFailure);
        }

        public void SendInvite(string channelId, InviteContent customInviteContent, Action onComplete, Action onCancel,
            Action<GetSocialError> onFailure)
        {
            SendInvite(channelId, customInviteContent, null, onComplete, onCancel, onFailure);
        }

        public void SendInvite(string channelId, InviteContent customInviteContent, LinkParams linkParams, Action onComplete,
            Action onCancel, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), channelId, customInviteContent, linkParams,
                onComplete, onCancel, onFailure);
            onComplete();
        }

        public bool RegisterInviteChannelPlugin(string channelId, InviteChannelPlugin inviteChannelPlugin)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), channelId, inviteChannelPlugin);
            return false;
        }

        public void GetReferralData(Action<ReferralData> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess, onFailure);
            onSuccess(null);
        }

        public void GetReferredUsers(Action<List<ReferredUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getReferredUsers");
            WithHadesClient(client =>
            {
                var rpcUsers = client.getReferredUsers(SessionId);
                var users = rpcUsers.ConvertAll(user => user.ToReferredUser());
                Ui(() =>
                {
                    LogResponse("getReferredUsers", rpcUsers.ToDebugString());
                    onSuccess.SafeCall(users);
                });
            }, onFailure);
        }

        public void GetReferredUsers(ReferralUsersQuery query, Action<List<ReferralUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getReferredUsers, query= ", query.ToString());
            WithHadesClient(client =>
            {
                var rpcUsers = client.getReferredUsersV2(SessionId, query.GetEventName(), query.GetOffset(), query.GetLimit());
                var users = rpcUsers.ConvertAll(user => user.ToReferralUser());
                Ui(() =>
                {
                    LogResponse("getReferredUsers", rpcUsers.ToDebugString());
                    onSuccess.SafeCall(users);
                });
            }, onFailure);
        }

        public void GetReferrerUsers(ReferralUsersQuery query, Action<List<ReferralUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getReferrerUsers, query= ", query.ToString());
            WithHadesClient(client =>
            {
                var rpcUsers = client.getReferrerUsers(SessionId, query.GetEventName(), query.GetOffset(), query.GetLimit());
                var users = rpcUsers.ConvertAll(user => user.ToReferralUser());
                Ui(() =>
                {
                    LogResponse("getReferrerUsers", rpcUsers.ToDebugString());
                    onSuccess.SafeCall(users);
                });
            }, onFailure);
        }
        public void CreateInviteLink(LinkParams linkParams, Action<string> onSuccess, Action<GetSocialError> onFailure)
        {
            var request = new THCreateTokenRequest
            {
                ProviderId = InvitesConsts.ManualProviderId,
                LinkParams = linkParams.ToDictionaryOfStrings()
            };
            
            LogRequest("createInviteUrl", "providerId=" + request.ProviderId + "; linkParams=" + request.LinkParams.ToDebugString());
            WithHadesClient(client =>
            {
                var response = client.createInviteUrl(SessionId, request);
                Ui(() =>
                {
                    LogResponse("createInviteUrl", response);
                    onSuccess.SafeCall(response.Url);
                });
            }, onFailure);
        }

        public void SetReferrer(string referrerId, string eventName, Dictionary<string, string> customData, Action onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("setReferrer", "referrerId=" + referrerId + "; eventName=" + eventName + "; customData=" + customData.ToDebugString());
            WithHadesClient(client =>
            {
                var response = client.setReferrer(SessionId, referrerId, eventName, customData);
                Ui(() =>
                {
                    LogResponse("setReferrer", response);
                    onSuccess.SafeCall();
                });
            }, onFailure);
        }

        public void TrackAnalyticsEvents(List<AnalyticsEvent> events)
        {
            var superProperties = _stateController.SuperProperties;
            var rpcEvents = events.ConvertAll(e => e.ShiftTime(_stateController.ServerTimeDiff).ToRpcModel());
            LogRequest("trackAnalyticsEvents", "events=" + rpcEvents.ToDebugString() + "\nsuperProperties=" + superProperties);
            
            WithHadesClient(client =>
            {
                var response = client.trackAnalyticsEvents(SessionId, superProperties, rpcEvents);
                Ui(() => LogResponse("trackAnalyticsEvents", response));
            });
        }
        
        public void RegisterForPushNotifications()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
        }

        public void SetNotificationListener(Func<Notification, bool, bool> listener)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), listener);
        }

        public void GetNotifications(NotificationsQuery query, Action<List<Notification>> onSuccess, Action<GetSocialError> onError)
        {
            var rpcQuery = query.ToRpcModel();
            
            LogRequest("getNotifications", rpcQuery);
            WithHadesClient(client =>
            {
                var response = client.getNotificationsList(SessionId, rpcQuery);
                Ui(() =>
                {
                    LogResponse("getNotifications", response.ToDebugString());
                    onSuccess.SafeCall(response.ConvertAll(notification => notification.FromRpcModel()));
                });
            }, onError);
        }

        public void GetNotificationsCount(NotificationsCountQuery query, Action<int> onSuccess, Action<GetSocialError> onError)
        {
            var rpcQuery = query.ToRpcModel();
            
            LogRequest("getNotificationsCount", rpcQuery);
            WithHadesClient(client =>
            {
                var response = client.getNotificationsCount(SessionId, rpcQuery);
                Ui(() =>
                {
                    LogResponse("getNotificationsCount", response);
                    onSuccess.SafeCall(response);
                });
            }, onError);
        }

        public void SetNotificationsStatus(List<string> notificationsIds, string status, Action onSuccess,
            Action<GetSocialError> onError)
        {
            var rpcBody = new THNotificationsSetStatusParams {
                Ids = notificationsIds,
                Status = status
            };
            
            LogRequest("setNotificationsStatus", rpcBody);
            WithHadesClient(client =>
            {
                var response = client.setNotificationsStatus(SessionId, rpcBody);
                Ui(() =>
                {
                    LogResponse("setNotificationsStatus", response);
                    onSuccess.SafeCall();
                });
            }, onError);
        }

        public void SetPushNotificationsEnabled(bool isEnabled, Action onSuccess, Action<GetSocialError> onError)
        {
            LogRequest("setPushNotificationsEnabled", isEnabled);
            WithHadesClient(client =>
            {
                var response = client.setPushNotificationsEnabled(SessionId, isEnabled);
                Ui(() =>
                {
                    LogResponse("setPushNotificationsEnabled", response);
                    onSuccess.SafeCall();
                });
            }, onError);
        }

        public void IsPushNotificationsEnabled(Action<bool> onSuccess, Action<GetSocialError> onError)
        {
            LogRequest("isPushNotificationsEnabled");
            WithHadesClient(client =>
            {
                var response = client.isPushNotificationsEnabled(SessionId);
                Ui(() =>
                {
                    LogResponse("isPushNotificationsEnabled", response);
                    onSuccess.SafeCall(response);
                });
            }, onError);
        }

        public bool SetOnUserChangedListener(Action listener)
        {
            _stateController.OnUserChanged = listener;
            return true;
        }

        public bool RemoveOnUserChangedListener()
        {
            _stateController.OnUserChanged = null;
            return true;
        }

        public string UserId
        {
            get { return IsInitialized ? User.Id : null; }
        }

        public bool IsUserAnonymous
        {
            get { return !IsInitialized || User.Identities == null || User.Identities.Count == 0; }
        }
        
        public void ResetUser(Action onSuccess, Action<GetSocialError> onError)
        {
            var request = new THSdkAuthRequest
            {
                AppId = _stateController.AppId,
                SessionProperties = _stateController.SuperProperties
            };
            
            LogRequest("authenticateSdk", request);
            
            WithHadesClient(client =>
            {
                var response = client.authenticateSdk(request);

                Ui(() =>
                {
                    LogResponse("authenticateSdk", response);
                    _stateController.SaveSession(response.SessionId, response.User);
                    onSuccess.SafeCall();
                });
            }, onError);
        }

        public Dictionary<string, string> UserAuthIdentities
        {
            get
            {
                return IsInitialized 
                    ? User.Identities.IdentitiesToDictionary() 
                    : new Dictionary<string, string>();
            }
        } 
        
        public Dictionary<string, string> AllPublicProperties
        {
            get 
            { 
                return IsInitialized && User.PublicProperties != null 
                        ? new Dictionary<string, string>(User.PublicProperties) 
                        : new Dictionary<string, string>(); 
            }
        }
        
        public Dictionary<string, string> AllPrivateProperties
        {
            get 
            { 
                return IsInitialized && User.PrivateProperties != null 
                        ? new Dictionary<string, string>(User.PrivateProperties) 
                        : new Dictionary<string, string>(); 
            }
        }
        
        public string DisplayName
        {
            get { return IsInitialized ? User.DisplayName : null; }
        }
        
        public void SetDisplayName(string displayName, Action onComplete, Action<GetSocialError> onFailure)
        {
            UpdateUser(new THPrivateUser{ DisplayName = displayName }, onComplete, onFailure);
        }

        public string AvatarUrl
        {
            get { return IsInitialized ? User.AvatarUrl : null; }
        }
        
        public void SetAvatarUrl(string avatarUrl, Action onComplete, Action<GetSocialError> onFailure)
        {
            UpdateUser(new THPrivateUser{ AvatarUrl = avatarUrl }, onComplete, onFailure);
        }

        public void SetAvatar(Texture2D avatar, Action onComplete, Action<GetSocialError> onFailure)
        {
            GetSocialLogs.W("Uploading Texture2D as avatar is not supported in Editor yet");
            onComplete();
        }

        public void SetPublicProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
            UpdateUser(new THPrivateUser{ PublicProperties = new Dictionary<string, string>
            {
                {key, value}
            }}, onSuccess, onFailure);
        }

        public void SetPrivateProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
            UpdateUser(new THPrivateUser{ PrivateProperties = new Dictionary<string, string>
            {
                {key, value}
            }}, onSuccess, onFailure);
        }

        public void RemovePublicProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
            SetPublicProperty(key, "", onSuccess, onFailure);
        }

        public void RemovePrivateProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
            SetPrivateProperty(key, "", onSuccess, onFailure);
        }

        public string GetPublicProperty(string key)
        {
            if (!IsInitialized) return null;
            string val;
            return AllPublicProperties.TryGetValue(key, out val) ? val : null;
        }

        public string GetPrivateProperty(string key)
        {
            if (!IsInitialized) return null;
            string val;
            return AllPrivateProperties.TryGetValue(key, out val) ? val : null;
        }

        private void UpdateUser(THPrivateUser user, Action onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("updateUser", user);
            WithHadesClient(client =>
            {
                User = client.updateUser(SessionId, user);
                Ui(() =>
                {
                    LogResponse("updateUser", User);
                    onSuccess.SafeCall();
                });
            }, onFailure);
        }

        public bool HasPublicProperty(string key)
        {
            return GetPublicProperty(key) != null;
        }

        public bool HasPrivateProperty(string key)
        {
            return GetPrivateProperty(key) != null;
        }

        public void AddAuthIdentity(AuthIdentity authIdentity, Action onComplete, Action<GetSocialError> onFailure, Action<ConflictUser> onConflict)
        {
            var identity = authIdentity.ToRpcModel();
            
            LogRequest("addIdentity", identity);
            
            WithHadesClient(client =>
            {
                try
                {
                    User = client.addIdentity(SessionId, authIdentity.ToRpcModel());
                    Ui(() =>
                    {
                        LogResponse("addIdentity", User);
                        onComplete.SafeCall();
                    });
                }
                catch (THErrors errors)
                {
                    var errorCode = errors.Errors.First().ErrorCode;
                    if (errorCode == THErrorCode.EMResourceAlreadyExists ||
                        errorCode == THErrorCode.IdentityAlreadyExists)
                    {
                        var conflictUser = client.getPrivateUserByIdentity(SessionId, identity).ToConflictUser();
                        Ui(() =>
                        {
                            LogResponse("addIdentity", "Conflict user: " + conflictUser);
                            onConflict.SafeCall(conflictUser);
                        });
                    }
                }
            }, onFailure);
        }

        public void RemoveAuthIdentity(string providerId, Action onSuccess, Action<GetSocialError> onFailure)
        {
            var identity = new THIdentity {Provider = providerId};
            LogRequest("removeIdentity", identity);
            
            WithHadesClient(client =>
            {
                User = client.removeIdentity(SessionId, identity);
                Ui(() =>
                {
                    LogResponse("removeIdentity", User);
                    onSuccess.SafeCall();
                });
            }, onFailure);
        }

        public void SwitchUser(AuthIdentity authIdentity, Action onSuccess, Action<GetSocialError> onFailure)
        {
            var request = new THSdkAuthRequest
            {
                AppId = _stateController.AppId,
                SessionProperties = _stateController.SuperProperties
            };

            var identity = authIdentity.ToRpcModel();
            
            LogRequest("switchUser", identity);
            
            WithHadesClient(client =>
            {
                var user = client.getPrivateUserByIdentity(SessionId, identity);

                request.UserId = user.Id;
                request.Password = user.Password;
                
                var response = client.authenticateSdk(request);

                Ui(() =>
                {
                    LogResponse("switchUser", response);
                    _stateController.SaveSession(response.SessionId, response.User);
                    onSuccess.SafeCall();
                });
            }, onFailure);
        }

        public void GetUserById(string userId, Action<PublicUser> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getUserById", "userId=" + userId);
            WithHadesClient(client =>
            {
                var user = client.getPublicUser(SessionId, userId);
                var publicUser = user.ToPublicUser();
                Ui(() =>
                {
                    LogResponse("getUserById", user);
                    onSuccess.SafeCall(publicUser);
                });
            }, onFailure);
        }

        public void GetUserByAuthIdentity(string providerId, string providerUserId, Action<PublicUser> onSuccess, Action<GetSocialError> onFailure)
        {
            GetUsersByAuthIdentities(providerId, new List<string>{ providerUserId }, users =>
            {
                if (users.Count == 1)
                {
                    Ui(() => onSuccess.SafeCall(users.Values.First()));
                }
                else
                {
                    var error = users.Count == 0
                        ? new GetSocialError(ErrorCodes.IllegalArgument,
                            "No GetSocial User found for provided arguments")
                        : new GetSocialError(ErrorCodes.IllegalState,
                            "API returned unexpected amount of responses");
                    Ui(() => onFailure.SafeCall(error));
                }
            }, onFailure);
        }

        public void GetUsersByAuthIdentities(string providerId, List<string> providerUserIds, Action<Dictionary<string, PublicUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getPublicUsersByIdentity", "providerId=" + providerId + "; userIds=" + providerUserIds.ToDebugString());
            WithHadesClient(client =>
            {
                var apiUsers = client.getPublicUsersByIdentity(SessionId, providerId, providerUserIds);
                var publicUsers = apiUsers.ToDictionary(k => k.Key, k => k.Value.ToPublicUser());
                Ui(() =>
                {
                    LogResponse("getPublicUsersByIdentity", apiUsers.ToDebugString());
                    onSuccess.SafeCall(publicUsers);
                });
            });
        }

        public void FindUsers(UsersQuery query, Action<List<UserReference>> onSuccess, Action<GetSocialError> onFailure)
        {
            var rpcQuery = query.ToRpcModel();
            LogRequest("findUsers", rpcQuery);
            WithHadesClient(client =>
            {
                var response = client.findUsers(SessionId, rpcQuery);
                Ui(() =>
                {
                    LogResponse("findUsers", response.ToDebugString());
                    onSuccess.SafeCall(response.ConvertAll(user => user.ToUserReference()));
                });
            }, onFailure);
        }

        public void AddFriend(string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("addFriend", userId);
            WithHadesClient(client =>
            {
                var response = client.addFriend(SessionId, userId);
                Ui(() =>
                {
                    LogResponse("addFriend", response);
                    onSuccess.SafeCall(response);
                });
            }, onFailure);
        }

        public void AddFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("addFriendsByIdentity", string.Format("providerId:{0}, userIds: {1}", providerId, providerUserIds.ToDebugString()));
            WithHadesClient(client =>
            {
                var response = client.addFriendsByIdentity(SessionId, providerId, providerUserIds);
                Ui(() =>
                {
                    LogResponse("addFriendsByIdentity", response);
                    onSuccess.SafeCall(response);
                });
            }, onFailure);
        }

        public void RemoveFriend(string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            
            LogRequest("removeFriend", userId);
            WithHadesClient(client =>
            {
                var response = client.removeFriend(SessionId, userId);
                Ui(() =>
                {
                    LogResponse("removeFriend", response);
                    onSuccess.SafeCall(response);
                });
            }, onFailure);
        }

        public void RemoveFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("removeFriendsByIdentity", string.Format("providerId:{0}, userIds: {1}", providerId, providerUserIds.ToDebugString()));
            WithHadesClient(client =>
            {
                var response = client.removeFriendsByIdentity(SessionId, providerId, providerUserIds);
                Ui(() =>
                {
                    LogResponse("removeFriendsByIdentity", response);
                    onSuccess.SafeCall(response);
                });
            }, onFailure);
        }

        public void SetFriends(List<string> userIds, Action onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("setFriends", userIds.ToDebugString());
            WithHadesClient(client =>
            {
                var response = client.setFriends(SessionId, userIds);
                Ui(() =>
                {
                    LogResponse("setFriends", response);
                    onSuccess.SafeCall();
                });
            }, onFailure);
        }

        public void SetFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("setFriendsByIdentity", string.Format("providerId:{0}, userIds: {1}", providerId, providerUserIds.ToDebugString()));
            WithHadesClient(client =>
            {
                var response = client.setFriendsByIdentity(SessionId, providerId, providerUserIds);
                Ui(() =>
                {
                    LogResponse("setFriendsByIdentity", response);
                    onSuccess.SafeCall();
                });
            }, onFailure);
        }

        public void IsFriend(string userId, Action<bool> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("isFriend", userId);
            WithHadesClient(client =>
            {
                var response = client.isFriend(SessionId, userId);
                Ui(() =>
                {
                    LogResponse("isFriend", response);
                    onSuccess.SafeCall(response);
                });
            }, onFailure);
        }

        public void GetFriendsCount(Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getFriendsCount");
            WithHadesClient(client =>
            {
                var response = client.getFriendsCount(SessionId);
                Ui(() =>
                {
                    LogResponse("getFriendsCount", response);
                    onSuccess.SafeCall(response);
                });
            }, onFailure);
        }

        public void GetFriends(int offset, int limit, Action<List<PublicUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getFriends", string.Format("offset: {0}, limit: {1}", offset, limit));
            WithHadesClient(client =>
            {
                var response = client.getFriends(SessionId, offset, limit);
                Ui(() =>
                {
                    LogResponse("getFriends", response.ToDebugString());
                    onSuccess.SafeCall(response.ConvertAll(user => user.ToPublicUser()));
                });
            }, onFailure);
        }

        public void GetSuggestedFriends(int offset, int limit, Action<List<SuggestedFriend>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getSuggestedFriends", string.Format("offset: {0}, limit: {1}", offset, limit));
            WithHadesClient(client =>
            {
                var response = client.getSuggestedFriends(SessionId, offset, limit);
                Ui(() =>
                {
                    LogResponse("getSuggestedFriends", response.ToDebugString());
                    onSuccess.SafeCall(response.ConvertAll(user => user.ToSuggestedFriend()));
                });
            }, onFailure);
        }

        public void GetFriendsReferences(Action<List<UserReference>> onSuccess, Action<GetSocialError> onFailure)
        {
            
            LogRequest("getFriendsReferences");
            WithHadesClient(client =>
            {
                var response = client.getMentionFriends(SessionId);
                Ui(() =>
                {
                    LogResponse("getFriendsReferences", response.ToDebugString());
                    onSuccess.SafeCall(response.ConvertAll(user => user.ToUserReference()));
                });
            }, onFailure);
        }

        public void GetAnnouncements(string feed, Action<List<ActivityPost>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getAnnouncements", feed);
            WithHadesClient(client =>
            {
                var response = client.getStickyActivities(SessionId, GetFeedName(feed));
                Ui(() =>
                {
                    LogResponse("getAnnouncements", response.ToDebugString());
                    onSuccess.SafeCall(response.ConvertAll(activity => activity.FromRpcModel()));
                });
            }, onFailure);
        }

        public void GetActivities(ActivitiesQuery query, Action<List<ActivityPost>> onSuccess, Action<GetSocialError> onFailure)
        {
            var rpcQuery = query.ToRpcModel();
            LogRequest("getActivities", rpcQuery);
            WithHadesClient(client =>
            {
                var response = client.getActivities(SessionId, GetFeedName(query._feed), rpcQuery);
                Ui(() =>
                {
                    LogResponse("getActivities", response.ToDebugString());
                    onSuccess.SafeCall(response.ConvertAll(activity => activity.FromRpcModel()));
                });
            }, onFailure);
        }

        private static string GetFeedName(string feedName)
        {
            return ActivitiesQuery.GlobalFeed.Equals(feedName) ? feedName : "s-" + feedName;
        }

        public void GetActivity(string activityId, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getActivity", activityId);
            WithHadesClient(client =>
            {
                var response = client.getActivity(SessionId, activityId);
                Ui(() =>
                {
                    LogResponse("getActivity", response);
                    onSuccess.SafeCall(response.FromRpcModel());
                });
            }, onFailure);
        }

        public void PostActivityToFeed(string feed, ActivityPostContent content, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
            var rpcContent = content.ToRpcModel();
            LogRequest("postActivity", string.Format("feed: {0}, content: {1}", feed, rpcContent));
            WithHadesClient(client =>
            {
                var response = client.postActivity(SessionId, GetFeedName(feed), rpcContent);
                Ui(() =>
                {
                    LogResponse("postActivity", response);
                    onSuccess.SafeCall(response.FromRpcModel());
                });
            }, onFailure);
        }

        public void PostCommentToActivity(string activityId, ActivityPostContent comment, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
            var rpcContent = comment.ToRpcModel();
            LogRequest("postComment", string.Format("activityId: {0}, content: {1}", activityId, rpcContent));
            WithHadesClient(client =>
            {
                var response = client.postComment(SessionId, activityId, rpcContent);
                Ui(() =>
                {
                    LogResponse("postComment", response);
                    onSuccess.SafeCall(response.FromRpcModel());
                });
            }, onFailure);
        }

        public void LikeActivity(string activityId, bool isLiked, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("likeActivity", string.Format("activityId: {0}, isLiked: {1}", activityId, isLiked));
            WithHadesClient(client =>
            {
                var response = client.likeActivity(SessionId, activityId, isLiked);
                Ui(() =>
                {
                    LogResponse("likeActivity", response);
                    onSuccess.SafeCall(response.FromRpcModel());
                });
            }, onFailure);
        }

        public void GetActivityLikers(string activityId, int offset, int limit, Action<List<PublicUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("getActivityLikers", string.Format("activityId: {0}, offset: {1}, limit: {2}", activityId, offset, limit));
            WithHadesClient(client =>
            {
                var response = client.getActivityLikers(SessionId, activityId, offset, limit);
                Ui(() =>
                {
                    LogResponse("getActivityLikers", response.ToDebugString());
                    onSuccess.SafeCall(response.ConvertAll(user => user.ToPublicUser()));
                });
            }, onFailure);
        }

        public void ReportActivity(string activityId, ReportingReason reportingReason, Action onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("reportActivity", string.Format("activityId: {0}, reportingReason: {1}", activityId, reportingReason));
            WithHadesClient(client =>
            {
                var response = client.reportActivity(SessionId, activityId, reportingReason.ToRpcModel());
                Ui(() =>
                {
                    LogResponse("reportActivity", response);
                    onSuccess.SafeCall();
                });
            }, onFailure);
        }

        public void RemoveActivities(List<string> activityIds, Action onSuccess, Action<GetSocialError> onFailure)
        {
            LogRequest("removeActivities", activityIds.ToDebugString());
            WithHadesClient(client =>
            {
                var response = client.removeActivities(SessionId, activityIds);
                Ui(() =>
                {
                    LogResponse("removeActivities", response);
                    onSuccess.SafeCall();
                });
            }, onFailure);
        }

        public void ClearReferralData()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
        }

        public void SetNotificationListener(NotificationListener listener)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), listener);
        }

        public void SetPushTokenListener(PushTokenListener listener)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), listener);
        }

        public void SendNotification(List<string> userIds, NotificationContent content, Action<NotificationsSummary> onSuccess, Action<GetSocialError> onError)
        {
            var rpcContent = content.ToRpcModel();
            
            LogRequest("sendNotification", string.Format("userIds: {0}, content: {1}", userIds.ToDebugString(), rpcContent));

            rpcContent.UserIds = userIds;

            WithHadesClient(client =>
            {
                var response = client.sendPushNotification(SessionId, rpcContent);
                Ui(() =>
                {
                    LogResponse("sendNotification", response);
                    onSuccess.SafeCall(response.FromRpcModel());
                });
            }, onError);
        }

        public void SetUserDetails(UserUpdate userUpdate, Action onSuccess, Action<GetSocialError> onFailure)
        {
            if (userUpdate._avatar != null) {
                GetSocialLogs.W("Uploading Texture2D as avatar is not supported in Editor yet");
            }
            UpdateUser(new THPrivateUser 
            {
                DisplayName = userUpdate._displayName,
                AvatarUrl = userUpdate._avatarUrl,
                PublicProperties = userUpdate._publicProperties,
                PrivateProperties = userUpdate._privateProperties,
                InternalPublicProperties = userUpdate._publicInternalProperties,
                InternalPrivateProperties = userUpdate._privateInternalProperties
            }, onSuccess, onFailure);
        }

        public void CreatePromoCode(PromoCodeBuilder promoCodeBuilder, Action<PromoCode> onSuccess, Action<GetSocialError> onError)
        {
            var rpcBuilder = promoCodeBuilder.ToRpcModel();
            LogRequest("createPromoCode", rpcBuilder);
            WithHadesClient(client =>
            {
                var response = client.setPromoCode(SessionId, rpcBuilder);
                Ui(() =>
                {
                    LogResponse("createPromoCode", response);
                    onSuccess.SafeCall(response.FromRpcModel());
                });
            }, onError);
        }

        public void GetPromoCode(string code, Action<PromoCode> onSuccess, Action<GetSocialError> onError)
        {
            LogRequest("getPromoCode", code);
            WithHadesClient(client =>
            {
                var response = client.getPromoCode(SessionId, code);
                Ui(() =>
                {
                    LogResponse("getPromoCode", response);
                    onSuccess.SafeCall(response.FromRpcModel());
                });
            }, onError);
        }

        public void ClaimPromoCode(string code, Action<PromoCode> onSuccess, Action<GetSocialError> onError)
        {
            LogRequest("claimPromoCode", code);
            WithHadesClient(client =>
            {
                var response = client.claimPromoCode(SessionId, code);
                Ui(() =>
                {
                    LogResponse("claimPromoCode", response);
                    onSuccess.SafeCall(response.FromRpcModel());
                });
            }, onError);
        }

        public bool TrackPurchaseEvent(PurchaseData purchaseData)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), purchaseData);
            return false;
        }

        public bool TrackCustomEvent(string customEvent, Dictionary<string, string> eventProperties)
        {
            TrackAnalyticsEvent(
                new AnalyticsEvent
                {
                    Name = customEvent,
                    CreatedAt = DateTime.Now,
                    Properties = eventProperties
                }
            );
            return true;
        }

        public void TrackAnalyticsEvent(AnalyticsEvent analyticsEvent) 
        {
            TrackAnalyticsEvents(new List<AnalyticsEvent> { analyticsEvent });
        }
        

        public void ProcessAction(GetSocialAction notificationAction)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), notificationAction);
        }


        public void Reset()
        {
            //
        }

        public void HandleOnStartUnityEvent()
        {
            GetSocialLogs.D("Not needed in OSX implementation");
        }

        private void UpdateSession()
        {
            var superProperties = _stateController.SuperProperties;
            if (IsInitialized)
            {
                LogRequest("updateSession", superProperties);
                WithHadesClient(client =>
                {
                    var response = client.updateSession(SessionId, superProperties);
                    Ui(() => LogResponse("updateSession", response));
                });
            }
        }

        private void WithHadesClient(Action<Hades.Client> whenReady, Action<GetSocialError> onError = null, bool requireInitialization = true)
        {
            if (requireInitialization && !IsInitialized)
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
                        _stateController.OnError.SafeCall(getSocialError);
                        onError.SafeCall(getSocialError);
                    });
                };
                try
                {
                    whenReady(client);
                } 
                catch (THErrors error)
                {
                    var first = error.Errors.First();
                    handleError(new GetSocialError((int) first.ErrorCode, first.ErrorMsg));
                }
                catch (Exception exception)
                {
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
                GetSocialLogs.D(log);
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
