using System;
using GetSocialSdk.Core;
using UnityEngine;

public class ChatsSection : BaseListSection<SimplePagingQuery, Chat>
{

    protected override void Count(SimplePagingQuery query, Action<int> success, Action<GetSocialError> error)
    {
        // not implemented
    }

    protected override SimplePagingQuery CreateQuery(QueryObject queryObject)
    {
        return new SimplePagingQuery();
    }

    protected override void DrawItem(Chat item)
    {
        GUILayout.Label(item.Title, GSStyles.BigLabelText);
        var lastMessageText = "";
        if (item.LastMessage != null)
        {
            lastMessageText = item.LastMessage.Text;
        }
        GUILayout.Label(lastMessageText, GSStyles.NormalLabelText);
        GUILayout.Label(item.AvatarUrl, GSStyles.NormalLabelText);
        DemoGuiUtils.DrawButton("Actions", () =>
        {
            ShowActions(item);
        }, style: GSStyles.Button);
    }

    protected override string GetSectionName()
    {
        return "Chats";
    }

    protected override bool HasQuery()
    {
        return false;
    }

    protected override bool IsBackButtonActive()
    {
        return true;
    }

    protected override void Load(PagingQuery<SimplePagingQuery> query, Action<PagingResult<Chat>> success, Action<GetSocialError> error)
    {
        Communities.GetChats(query.Query, success, error);
    }

    private void ShowActions(Chat chat)
    {
        var popup = Dialog().WithTitle("Actions");
        popup.AddAction("Info", () => Print(chat));
        popup.AddAction("Open Chat", () => Chat(chat));
        popup.AddAction("Cancel", () => { });
        popup.Show();
    }

    private void Chat(Chat chat)
    {
        demoController.PushMenuSection<ChatMessagesView>(section =>
        {
            var chatId = ChatId.Create(chat.Id);
            section.SetChatId(chatId);
        });
    }

    private void Print(Chat chat)
    {
        _console.LogD(chat.ToString());
    }

}
