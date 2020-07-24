using UnityEngine;
using System.Collections;
using GetSocialSdk.Core;
using System.Collections.Generic;

public class SetReferrerSection : DemoMenuSection
{
    string _referrerId = "";
    string _eventName = "";
    string _key1 = "key1", _key2 = "key2", _key3 = "key3";
    string _value1 = "value1", _value2 = "value2", _value3 = "value3";

    #region implemented abstract members of DemoMenuSection

    protected override string GetTitle()
    {
        return "Set Referrer";
    }

    protected override void DrawSectionBody()
    {
        DrawSetReferrerForm();
    }

    #endregion

    public void DrawSetReferrerForm()
    {
        GUILayout.BeginVertical("box");

        GUILayout.Label("Set Referrer", GSStyles.NormalLabelText);

        // Subject
        GUILayout.BeginHorizontal();
        GUILayout.Label("Referrer Id", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        _referrerId = GUILayout.TextField(_referrerId, GSStyles.TextField,
            GUILayout.Width(Screen.width * 0.50f));
        if (GUILayout.Button("Paste", GSStyles.PasteButton))
        {
            _referrerId = GUIUtility.systemCopyBuffer;
        }
        GUILayout.EndHorizontal();

        // Text
        GUILayout.BeginHorizontal();
        GUILayout.Label("Event", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        _eventName = GUILayout.TextField(_eventName, GSStyles.TextField, GUILayout.Width(Screen.width * 0.50f));
        if (GUILayout.Button("Paste", GSStyles.PasteButton))
        {
            _eventName = GUIUtility.systemCopyBuffer;
        }

        GUILayout.EndHorizontal();

        // Custom data
        GUILayout.Label("Custom Data (optional)", GSStyles.NormalLabelText);
        DrawKeyValuePair(ref _key1, ref _value1);
        DrawKeyValuePair(ref _key2, ref _value2);
        DrawKeyValuePair(ref _key3, ref _value3);

        GUILayout.EndVertical();

        GUILayout.Space(20);
        DemoGuiUtils.DrawButton("Set Referrer",
            () => CallSetReferrerMethod(),
            style: GSStyles.Button);

    }

    void DrawKeyValuePair(ref string key, ref string value)
    {
        GUILayout.BeginHorizontal();
        GSStyles.TextField.fixedWidth = Screen.width / 2 - 12f;

        if (string.IsNullOrEmpty(key))
        {
            key = "key";
        }
        if (string.IsNullOrEmpty(value))
        {
            value = "value";
        }
        key = GUILayout.TextField(key, GSStyles.TextField);
        value = GUILayout.TextField(value, GSStyles.TextField);

        GSStyles.TextField.fixedWidth = 0;
        GUILayout.EndHorizontal();
    }

    private void CallSetReferrerMethod()
    {
        var customData = new Dictionary<string, string>();
        customData[_key1] = _value1;
        customData[_key2] = _value2;
        customData[_key3] = _value3;
        Invites.SetReferrer(UserId.Create(_referrerId), _eventName, customData, () => {
            _console.LogD("Referrer was set");
        }, (error) => {
            _console.LogD("Failed to set referrer: " + error);
        });
    }
}
