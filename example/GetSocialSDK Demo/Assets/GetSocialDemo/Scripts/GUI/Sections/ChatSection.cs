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
using System.Collections.Generic;
using System;
using GetSocialSdk.Chat;
using UnityEngine;

public class ChatSection : DemoMenuSection
{
    private Dictionary<string, Action> sectionButtons;

    private List<IPrivateChatRoom> privateRooms = new List<IPrivateChatRoom>();

    #region implemented abstract members of DemoMenuSection
    protected override string GetTitle()
    {
        return "Chat";
    }

    protected override void InitGuiElements()
    {
        sectionButtons = new Dictionary<string, Action> {
            { "Open Global Chat", OpenGlobalChat },
            { "Open Conversation List", OpenConversationList }
        };
    }

    protected override void DrawSectionBody()
    {
        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButtons(sectionButtons, GSStyles.Button);
        GUILayout.EndHorizontal();

        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButton("Global public room", GetPublicGlobalRoom, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Refresh private rooms", ListPrivateRooms, true, GSStyles.Button);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButton("Msg to global room", SendTestMessageToGlobalRoom, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Typing to global room", SendTypingStatusToGlobalRoom, true, GSStyles.Button);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButton("Subscribe to global", SubscribeToGlobalRoom, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Unsubscribe from global", UnsubscribeFromGlobalRoom, true, GSStyles.Button);
        GUILayout.EndHorizontal();

        DemoGuiUtils.DrawButton("Global room get last messages", GetMessagesFromGlobalRoom, true, GSStyles.Button);

        GUILayout.Space(15);
        DrawPrivateRooms();
    }

    void DrawPrivateRooms()
    {
        if(privateRooms.Count == 0)
        {
            return;
        }

        GUILayout.Label("Private rooms", GSStyles.NormalLabelText);
        foreach(var room in privateRooms)
        {
            GUILayout.BeginHorizontal();
            DemoGuiUtils.DrawButton("Msg to: " + room.OtherParticipant.DisplayName, () => SendTestMessageToRoom(room), true, GSStyles.Button);
            DemoGuiUtils.DrawButton("Typing to: " + room.OtherParticipant.DisplayName, () => SendTestTypingStatusToRoom(room), true, GSStyles.Button);
            GUILayout.EndHorizontal();
        }
    }

    #endregion

    private void OpenGlobalChat()
    {
        GetSocialChat.Instance.CreateChatViewForRoomName("global").SetTitle("Global Chat").Show();
    }

    private void OpenConversationList()
    {
        GetSocialChat.Instance.CreateChatListView().Show();
    }

    #region chat_API
    private void GetPublicGlobalRoom()
    {
        GetPublicGlobalRoom(room => console.LogD("Received global public room: " + room),
            err => console.LogE(err));
    }

    private void SendTestMessageToGlobalRoom()
    {
        GetPublicGlobalRoom(room =>
        {
            SendTestMessageToRoom(room);
        }, err => console.LogE(err));
    }

    private void SendTypingStatusToGlobalRoom()
    {
        GetPublicGlobalRoom(room =>
        {
            SendTestTypingStatusToRoom(room);
        }, err => console.LogE(err));
    }

    private void GetMessagesFromGlobalRoom()
    {
        GetPublicGlobalRoom(room =>
        {
            room.GetMessages(room.LastMessage, 5, messages =>
            {
                foreach(var msg in messages)
                {
                    console.LogD(string.Format("Got message: {0}", msg));
                }
            }, err => console.LogE(err));
        },
            err => console.LogE(err));
    }

    private void ListPrivateRooms()
    {
        GetSocialChat.Instance.GetAllPrivateRooms(
            rooms =>
            {
                privateRooms = rooms;
                console.LogD(string.Format("{0} private chat rooms found.", rooms.Count));
                if(rooms.Count == 0)
                {
                    console.LogD("There are no private rooms for this user");
                }
                else
                {
                    foreach(var room in rooms)
                    {
                        console.LogD("Got room: " + room);
                    }
                }
            },
            err => console.LogE(err));
    }

    private void SendTestMessageToRoom(IChatRoom room)
    {
        room.SendMessage(ChatMessageContent.CreateWithText("test message"), 
            () => console.LogD("Send test message success"), 
            err => console.LogE("Send test message failed: " + err));
    }

    private void SendTestTypingStatusToRoom(IChatRoom room)
    {
        room.SetTypingStatus(TypingStatus.Typing, 
            () => console.LogD("Send test typing status success"), 
            err => console.LogE("Send test typing status failed: " + err));
    }

    private void SubscribeToGlobalRoom()
    {
        GetPublicGlobalRoom(room =>
        {
            room.Subscribe(
                () =>
                {
                    console.LogD("Subscribe to global room success");
                }, err => console.LogE(err));
        },
            err => console.LogE(err));
    }

    private void UnsubscribeFromGlobalRoom()
    {
        GetPublicGlobalRoom(room =>
        {
            room.Unsubscribe(
                () =>
                {
                    console.LogD("Unsubscribe from global room success");
                }, err => console.LogE(err));
        },
            err => console.LogE(err));
    }
    #endregion

    private static void GetPublicGlobalRoom(Action<IPublicChatRoom> onSuccess, Action<string> onFailure)
    {
        GetSocialChat.Instance.GetPublicRoom("global", onSuccess, onFailure);
    }
}
#endif
