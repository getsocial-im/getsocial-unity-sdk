using System;
using System.Collections.Generic;
using UnityEngine;
using GetSocialSdk.Core;
using Assets.GetSocialDemo.Scripts.Utils;

public class ChatMessagesView : DemoMenuSection
{
    public ChatId _chatId;
    private string _messageText;
    private bool _useCustomImage;
    private bool _useCustomVideo;
    private ChatMessagesPagingQuery _pagingQuery;
    private string _refreshCursor;
    private string _nextCursor;
    private string _previousCursor;

    public ChatMessagesView()
    {
    }

    public void Awake()
    {
        if (_console == null)
        {
            _console = gameObject.GetComponent<DemoAppConsole>().Init();
        }
    }

    public void SetChatId(ChatId chatId)
    {
        Debug.Log("Setting chatid: " + chatId);

        _chatId = chatId;
        LoadInitialMessages();
    }

    protected override bool IsBackButtonActive()
    {
        return true;
    }

    private List<ChatMessage> _messages;
    
    protected override void DrawSectionBody()
    {
        GUILayout.BeginHorizontal();
        _messageText = GUILayout.TextField(_messageText, GSStyles.TextField);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawRow(() =>
        {
            _useCustomImage = GUILayout.Toggle(_useCustomImage, "", GSStyles.Toggle);
            GUILayout.Label("Send Image", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        });
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawRow(() =>
        {
            _useCustomVideo = GUILayout.Toggle(_useCustomVideo, "", GSStyles.Toggle);
            GUILayout.Label("Send Video", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        });
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButton("Send", () =>
        {
            SendChatMessage();
        }, true, GSStyles.Button);

        DemoGuiUtils.DrawButton("Load Older", () =>
        {
            LoadOlderMessages();
        }, true, GSStyles.Button);

        DemoGuiUtils.DrawButton("Load Newer", () =>
        {
            LoadNewerMessages(true);
        }, true, GSStyles.Button);
        GUILayout.EndHorizontal();

        foreach (var message in _messages)
        {
            DemoGuiUtils.DrawRow(DrawMessage(message));
        }
    }

    private Action DrawMessage(ChatMessage message)
    {
        return () =>
        {
            GUILayout.Label("Author: " + message.Author.DisplayName, GSStyles.NormalLabelText);
            GUILayout.Label("Text: " + message.Text, GSStyles.NormalLabelText);
            DemoGuiUtils.DrawButton("Actions", () => ShowActions(message), style: GSStyles.Button);
        };
    }

    private void ShowActions(ChatMessage message)
    {
        var popup = Dialog().WithTitle("Actions");
        popup.AddAction("Info", () => _console.LogD(message.ToString()));
        popup.AddAction("Cancel", () => { });
        popup.Show();
    }

    protected override string GetTitle()
    {
        return "Chat with ";// + Receiver.DisplayName;
    }

    private void LoadInitialMessages()
    {
        _messages = new List<ChatMessage>();
        _pagingQuery = new ChatMessagesPagingQuery(ChatMessagesQuery.InChat(_chatId));
        LoadMessages((entries) => {
            _messages.AddAll(entries);
            LoadNewerMessages(false);
        });
    }

    private void LoadOlderMessages()
    {
        if (_previousCursor != null && _previousCursor.Length > 0)
        {
            _pagingQuery = _pagingQuery.WithPreviousMessagesCursor(_previousCursor);
            LoadMessages((entries) =>
            {
                _messages.InsertRange(0, entries);
            });
        }
    }

    private void LoadNewerMessages(bool refresh)
    {
        if (_nextCursor != null && _nextCursor.Length > 0)
        {
            _pagingQuery = _pagingQuery.WithNextMessagesCursor(_nextCursor);
            LoadMessages((entries) => {
                _messages.AddAll(entries);
                LoadNewerMessages(false);
            }); 
        } else if (_refreshCursor != null && refresh)
        {
            _pagingQuery = _pagingQuery.WithNextMessagesCursor(_refreshCursor);
            LoadMessages((entries) => {
                _messages.AddAll(entries);
                LoadNewerMessages(false);
            });
        }
    }

    public void LoadMessages(Action<List<ChatMessage>> action) 
    {
        Communities.GetChatMessages(_pagingQuery, (result) => {
            _previousCursor = result.PreviousMessagesCursor;
            _nextCursor = result.NextMessagesCursor;
            _refreshCursor = result.RefreshCursor;
            action(result.Messages);
        },
        (error) => {
                _console.LogE("Failed to get messages, error: " + error);
            }
        );
    }

    private void SendChatMessage()
    {
        var content = new ChatMessageContent();
        content.Text = _messageText;
        if (_useCustomImage)
        {
            content.AddMediaAttachment(MediaAttachment.WithImage(Resources.Load<Texture2D>("activityImage")));
        }
        if (_useCustomVideo)
        {
            content.AddMediaAttachment(MediaAttachment.WithVideo(DemoUtils.LoadSampleVideoBytes()));
        }
        Communities.SendChatMessage(content, _chatId,
            (message) => {
                _messageText = null;
                _useCustomImage = false;
                _useCustomVideo = false;
                LoadNewerMessages(true);
            }, (error) => {
                _console.LogE("Failed to send message, error: " + error);
            });
    }
}
