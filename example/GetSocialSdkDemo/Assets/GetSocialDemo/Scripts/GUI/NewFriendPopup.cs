using UnityEngine;
using System;
using System.Collections;
using GetSocialSdk.Core;

public class NewFriendPopup : MonoBehaviour
{
    private PublicUser _publicUser;
    private Texture2D _avatar;
    private Action _callback;

    public NewFriendPopup Init()
    {
        return this;
    }
    
    public void SetFriend(PublicUser publicUser)
    {
        _publicUser = publicUser;
        if(!string.IsNullOrEmpty(_publicUser.AvatarUrl))
        {
            StartCoroutine(DownloadImage(publicUser.AvatarUrl));
        }
    }
    
    public void Open(Action callback)
    {
        _callback = callback;
        
        int width = Screen.width / 2;
        int height = Screen.height / 2;

        GUIStyle popupStyle = new GUIStyle();
        popupStyle.normal.background = Texture2D.whiteTexture;
        
        GUI.Window(100, new Rect((Screen.width - width) / 2,(Screen.height - height) / 2, width, height),
            DrawContent,"", popupStyle);
    }

    private void DrawContent(int windowId)
    {
        GUILayout.Space(40);
        GUILayout.BeginVertical();
        GUILayout.Label("You have a new friend!", GSStyles.NormalLabelText);
        GUILayout.Space(40);
        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        GUILayout.Box(_avatar ?? Texture2D.blackTexture, GUILayout.Width(Screen.width / 5), GUILayout.Height(Screen.width / 5));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), _avatar ?? Texture2D.blackTexture, ScaleMode.StretchToFill);
        GUILayout.Space(40);
        GUILayout.Label(_publicUser.DisplayName, GSStyles.NormalLabelText);
        GUILayout.EndHorizontal();
        GUILayout.Space(40);
        if(GUILayout.Button("Close", GSStyles.Button)){
            GUI.BringWindowToBack(100);
            _callback();
        }
        GUILayout.EndVertical();
        GUI.BringWindowToFront(100);
    }

    IEnumerator DownloadImage(string url)
    {
        var www = new WWW(url);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("Failed to download avatar, reason: " + www.error);
        }
        if (www.isDone)
        {
            _avatar = www.texture;
        }
    } 
}