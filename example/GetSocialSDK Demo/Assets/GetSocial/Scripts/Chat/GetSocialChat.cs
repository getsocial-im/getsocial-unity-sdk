/**
 *     Copyright 2015-2016 GetSocial B.V.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#if ENABLE_GETSOCIAL_CHAT
using System;
using System.Collections.Generic;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.Chat
{
    /// <summary>
    /// Singleton class to interact with GetSocial chat.
    /// </summary>
    public sealed class GetSocialChat
    {
        private static GetSocialChat instance;

        private readonly IGetSocialNativeBridge getSocialImpl;
        private readonly IGetSocialChatNativeBridge getSocialChatImpl;

        private readonly List<IUnreadRoomCountChangedListener> unreadRoomCountChangedListeners;
        private readonly List<IChatMessageListener> messagesListeners;
        private readonly List<ITypingStatusListener> typingStatusListeners;

        #region initialization

        private GetSocialChat()
        {
            unreadRoomCountChangedListeners = new List<IUnreadRoomCountChangedListener>();
            messagesListeners = new List<IChatMessageListener>();
            typingStatusListeners = new List<ITypingStatusListener>();

            getSocialImpl = GetSocialFactory.InstantiateGetSocial();
            getSocialChatImpl = GetSocialChatFactory.InstantiateGetSocialChat();

            getSocialChatImpl.SetOnUnreadCountChangedListenerInternal(OnPublicRoomCountChange, OnPrivateRoomCountChange);
            getSocialChatImpl.SetMessagesListenerInternal(OnPublicRoomMessageReceived, OnPrivateRoomMessageReceived);
            getSocialChatImpl.SetTypingStatusListenerInternal(OnPublicRoomTypingStatusReceived, OnPrivateRoomTypingStatusReceived);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance of GetSocial chat.</value>
        public static GetSocialChat Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new GetSocialChat();
                }
                return instance;
            }
        }

        #endregion

        /// <summary>
        /// Check if chat feature is enabled on a GetSocial Dashboard.
        /// </summary>
        /// <value><c>true</c> if chat feature is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled
        {
            get
            {
                return getSocialChatImpl.IsEnabled;
            }
        }

        /// <summary>
        /// Gets the number of unread chat conversations.
        /// </summary>
        [Obsolete("Use UnreadPublicRoomsCount and UnreadPrivateRoomsCount instead")]
        public int UnreadConversationsCount
        {
            get
            {
                return UnreadPublicRoomsCount + UnreadPrivateRoomsCount;
            }
        }

        /// <summary>
        /// Get number of all public rooms that are marked as unread.
        /// </summary>
        /// <value>The number of all public rooms that are marked as unread.</value>
        public int UnreadPublicRoomsCount
        {
            get
            {
                return getSocialChatImpl.UnreadPublicRoomsCount;
            }
        }

        /// <summary>
        /// Get number of all private rooms that are marked as unread.
        /// </summary>
        /// <value>The number of all private rooms that are marked as unread.</value>
        public int UnreadPrivateRoomsCount
        {
            get
            {
                return getSocialChatImpl.UnreadPrivateRoomsCount;
            }
        }

        /// <summary>
        /// Gets public chat room by its name.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback.</param>
        public void GetPublicRoom(string name, Action<IPublicChatRoom> onSuccess, Action<string> onFailure)
        {
            Check.Argument.IsStrNotNullOrEmpty(name, "name", "Room name can't be null or empty");
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");
            Check.Argument.IsNotNull(onFailure, "onFailure", "Failure callback cannot be null");

            getSocialChatImpl.GetPublicRoom(name, onSuccess, onFailure);
        }

        /// <summary>
        /// Gets all private chat rooms
        /// </summary>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback.</param>
        public void GetAllPrivateRooms(Action<List<IPrivateChatRoom>> onSuccess, Action<string> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");
            Check.Argument.IsNotNull(onFailure, "onFailure", "Failure callback cannot be null");

            getSocialChatImpl.GetAllPrivateRooms(onSuccess, onFailure);
        }

        /// <summary>
        /// Gets private chat room with the other user.
        /// </summary>
        /// <param name="user">Other user to get private chat with.</param>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback.</param>
        public void GetPrivateRoom(User user, Action<IPrivateChatRoom> onSuccess, Action<string> onFailure)
        {
            Check.Argument.IsNotNull(user, "user", "User must not be null");
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");
            Check.Argument.IsNotNull(onFailure, "onFailure", "Failure callback cannot be null");

            getSocialChatImpl.GetPrivateRoom(user, onSuccess, onFailure);
        }

        /// <summary>
        /// Gets private chat room with the other user on provider.
        /// </summary>
        /// <param name="provider">Provider e.g. facebook.</param>
        /// <param name="userId">User id on provider.</param>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback.</param>
        public void GetPrivateRoom(string provider, string userId, Action<IPrivateChatRoom> onSuccess, Action<string> onFailure)
        {
            Check.Argument.IsStrNotNullOrEmpty(provider, "provider", "Provider can't be null or empty");
            Check.Argument.IsStrNotNullOrEmpty(userId, "userId", "User id can't be null or empty");
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");
            Check.Argument.IsNotNull(onFailure, "onFailure", "Failure callback cannot be null");

            getSocialChatImpl.GetPrivateRoom(provider, userId, onSuccess, onFailure);
        }

        // =======================================
        // ============= LISTENERS ===============
        // =======================================

        /// <summary>
        /// Unread conversations listener.
        /// </summary>
        [Obsolete("Use AddOnUnreadRoomCountChangedListener instead")]
        public void SetOnUnreadConversationsCountChangeListener(Action<int> onUnreadConversationsCountChange)
        {
            getSocialChatImpl.SetOnUnreadConversationsCountChangeListener(onUnreadConversationsCountChange);
        }

        #region unread_room_count_listener
        /// <summary>
        /// Adds the listener to listen for public rooms unread count changes.
        /// </summary>
        /// <param name="listener">Invoked every time when unread rooms count changes.</param>
        public void AddOnUnreadRoomCountChangedListener(IUnreadRoomCountChangedListener listener)
        {
            Check.Argument.IsNotNull(listener, "listener", "Listener cannot be null");

            if(!unreadRoomCountChangedListeners.Contains(listener))
            {
                unreadRoomCountChangedListeners.Add(listener);
            }
            else
            {
                Debug.LogWarning("This listener is already added");
            }
        }

        private void OnPublicRoomCountChange(int count)
        {
            foreach(var listener in unreadRoomCountChangedListeners)
            {
                listener.OnUnreadPublicRoomsCountChanged(count);
            }
        }

        private void OnPrivateRoomCountChange(int count)
        {
            foreach(var listener in unreadRoomCountChangedListeners)
            {
                listener.OnUnreadPrivateRoomsCountChanged(count);
            }
        }

        /// <summary>
        /// Removes the unread rooms count change listener.
        /// </summary>
        /// <param name="listener">Listener to remove.</param>
        public void RemoveOnUnreadRoomCountChangedListener(IUnreadRoomCountChangedListener listener)
        {
            Check.Argument.IsNotNull(listener, "listener", "Listener cannot be null");

            if(unreadRoomCountChangedListeners.Contains(listener))
            {
                unreadRoomCountChangedListeners.Remove(listener);
            }
            else
            {
                Debug.LogWarning("The listener you are trying to remove is not added.");
            }
        }
        #endregion

        #region messages_listener
        /// <summary>
        /// Adds the message listener to listen for room messages.
        /// </summary>
        /// <param name="listener">Invoked when room message received.</param>
        public void AddMessageListener(IChatMessageListener listener)
        {
            Check.Argument.IsNotNull(listener, "listener", "Listener cannot be null");

            if(!messagesListeners.Contains(listener))
            {
                messagesListeners.Add(listener);
            }
            else
            {
                Debug.LogWarning("This listener is already added");
            }
        }

        /// <summary>
        /// Removes messages listener.
        /// </summary>
        /// <param name="listener">Listener to remove.</param>
        public void RemoveMessageListener(IChatMessageListener listener)
        {
            Check.Argument.IsNotNull(listener, "listener", "Listener cannot be null");

            if(messagesListeners.Contains(listener))
            {
                messagesListeners.Remove(listener);
            }
            else
            {
                Debug.LogWarning("The listener you are trying to remove is not added.");
            }
        }

        private void OnPublicRoomMessageReceived(IPublicChatRoom publicRoom, ChatMessage message)
        {
            foreach(var listener in messagesListeners)
            {
                listener.OnPublicRoomMessage(publicRoom, message);
            }
        }

        private void OnPrivateRoomMessageReceived(IPrivateChatRoom privateRoom, ChatMessage message)
        {
            foreach(var listener in messagesListeners)
            {
                listener.OnPrivateRoomMessage(privateRoom, message);
            }
        }
        #endregion

        #region typing_status_listener
        /// <summary>
        /// Adds the typing status listener.
        /// </summary>
        /// <param name="listener">Callback invoked when user typing status changes.</param>
        public void AddTypingStatusListener(ITypingStatusListener listener)
        {
            Check.Argument.IsNotNull(listener, "listener", "Listener cannot be null");

            if(!typingStatusListeners.Contains(listener))
            {
                typingStatusListeners.Add(listener);
            }
            else
            {
                Debug.LogWarning("This listener is already added");
            }
        }

        /// <summary>
        /// Removes the typing status listener.
        /// </summary>
        /// <param name="listener">Listener to remove.</param>
        public void RemoveTypingStatusListener(ITypingStatusListener listener)
        {
            Check.Argument.IsNotNull(listener, "listener", "Listener cannot be null");

            if(typingStatusListeners.Contains(listener))
            {
                typingStatusListeners.Remove(listener);
            }
            else
            {
                Debug.LogWarning("The listener you are trying to remove is not added.");
            }
        }

        void OnPublicRoomTypingStatusReceived(IPublicChatRoom room, User user, TypingStatus typingStatus)
        {
            foreach(var listener in typingStatusListeners)
            {
                listener.OnPublicRoomTypingStatusReceived(room, user, typingStatus);
            }
        }

        void OnPrivateRoomTypingStatusReceived(IPrivateChatRoom room, User user, TypingStatus typingStatus)
        {
            foreach(var listener in typingStatusListeners)
            {
                listener.OnPrivateRoomTypingStatusReceived(room, user, typingStatus);
            }
        }
        #endregion

        // =======================================
        // ========== END LISTENERS ==============
        // =======================================

        #region view_builders

        /// <summary>
        /// Creates the chat list view.
        /// </summary>
        /// <returns><see cref="ChatListViewBuilder"/> instance.</returns>
        public ChatListViewBuilder CreateChatListView()
        {
            return ChatListViewBuilder.Construct(getSocialImpl);
        }

        /// <summary>
        /// Creates <see cref="ChatViewBuilder"/> used to open the Chat View.
        /// </summary>
        /// <param name="userId">The id of the user to chat with</param>
        /// <returns><see cref="ChatViewBuilder"/> instance.</returns>
        public ChatViewBuilder CreateChatViewForUserId(string userId)
        {
            Check.Argument.IsStrNotNullOrEmpty(userId, "userId", "User id can't be null or empty");

            return ChatViewBuilder.ConstructWithUserId(getSocialImpl, userId);
        }

        /// <summary>
        /// Creates <see cref="ChatViewBuilder"/> used to open the Chat View.
        /// </summary>
        /// <param name="userId">The id of the user to chat with</param>
        /// <param name="providerId">The id of the external provider</param>
        /// <returns><see cref="ChatViewBuilder"/> instance.</returns>
        public ChatViewBuilder CreateChatViewForUserIdOnProvider(string userId, string providerId)
        {
            Check.Argument.IsStrNotNullOrEmpty(userId, "userId", "User id can't be null or empty");
            Check.Argument.IsStrNotNullOrEmpty(providerId, "providerId", "Provider id can't be null or empty");

            return ChatViewBuilder.ConstructWithUserIdAndProviderId(getSocialImpl, userId, providerId);
        }

        /// <summary>
        /// Creates <see cref="ChatViewBuilder"/> used to open the Chat View.
        /// </summary>
        /// <param name="roomName">The name of the chat room</param>
        /// <returns><see cref="ChatViewBuilder"/> instance.</returns>
        public ChatViewBuilder CreateChatViewForRoomName(string roomName)
        {
            Check.Argument.IsStrNotNullOrEmpty(roomName, "roomName", "Room name can't be null or empty");

            return ChatViewBuilder.ConstructWithRoomName(getSocialImpl, roomName);
        }

        #endregion
    }
}
#endif
