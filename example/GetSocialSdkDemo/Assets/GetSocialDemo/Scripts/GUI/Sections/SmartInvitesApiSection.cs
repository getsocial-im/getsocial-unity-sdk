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

using System;
using System.Linq;
using GetSocialSdk.Core;
using UnityEngine;

public class SmartInvitesApiSection : DemoMenuSection
{
    // Custom invites
    string _customWindowTitle = "Custom Subject";

    string _customText = string.Format("It's an awesome app!, Download it from {0}.",
        InviteTextPlaceholders.PlaceholderAppInviteUrl);

    string _key1 = "key1", _key2 = "key2", _key3 = "key3";
    string _value1 = "value1", _value2 = "value2", _value3 = "value3";

    InviteChannel[] _currentInviteChannels = { };

    public string CustomTitle
    {
        get { return _customWindowTitle; }
    }

    public CustomReferralData CustomReferralData
    {
        get
        {
            return new CustomReferralData
            {
                {_key1, _value1},
                {_key2, _value2},
                {_key3, _value3}
            };
        }
    }

    public InviteContent CustomInviteContent
    {
        get
        {
            var randomImageUrl = string.Format("http://api.adorable.io/avatars/150/{0}", SystemInfo.deviceUniqueIdentifier);
            return InviteContent.CreateBuilder()
                .WithSubject(_customWindowTitle)
                .WithText(_customText)
                .WithImageUrl(randomImageUrl)
                .Build();
        }
    }

    #region implemented abstract members of DemoMenuSection

    protected override string GetTitle()
    {
        return "Smart Invites API";
    }

    protected override void DrawSectionBody()
    {
        DrawMainSection();
    }

    #endregion

    void DrawMainSection()
    {
        DemoGuiUtils.DrawButton("Get Supported Channels", GetSupportedInviteChannels, style: GSStyles.Button);
        DrawCustomInviteParamsForm();

        // API calls to SendInvite
        DrawReferralData();
        DrawSendInvites();
        DrawSendCustomInvites();
    }

    void GetSupportedInviteChannels()
    {
        _currentInviteChannels = GetSocial.InviteChannels;
        var channels = _currentInviteChannels.ToList().ConvertAll(x => x.ToString()).ToArray();
        var channelsJoined = string.Join(", ", channels);
        _console.LogD(string.Format("Available invite channels: {0}", channelsJoined));
    }

    public void DrawCustomInviteParamsForm()
    {
        GUILayout.BeginVertical("box");

        GUILayout.Label("Customize your invite subject and text (optional)", GSStyles.NormalLabelText);

        // Subject
        GUILayout.BeginHorizontal();
        GUILayout.Label("Subject", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        _customWindowTitle = GUILayout.TextField(_customWindowTitle, GSStyles.TextField,
            GUILayout.Width(Screen.width * 0.75f));
        GUILayout.EndHorizontal();

        // Text
        GUILayout.BeginHorizontal();
        GUILayout.Label("Text", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        _customText = GUILayout.TextField(_customText, GSStyles.TextField, GUILayout.Width(Screen.width * 0.75f));
        GUILayout.EndHorizontal();

        // Custom data
        GUILayout.Label("Attach some key/value pairs to your invite (optional)", GSStyles.NormalLabelText);
        DrawKeyValuePair(ref _key1, ref _value1);
        DrawKeyValuePair(ref _key2, ref _value2);
        DrawKeyValuePair(ref _key3, ref _value3);

        GUILayout.EndVertical();
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

    private void DrawReferralData()
    {
        GUILayout.Label("Referral data", GSStyles.BigLabelText);

        DemoGuiUtils.DrawButton("Get referral data", GetReferralData, style: GSStyles.Button);
    }

    private void GetReferralData()
    {
        GetSocial.GetReferralData(
            referralData => {
                var logMessage = String.Empty;

                if (referralData != null)
                {
                    logMessage += string.Format("Token: {0}\n", referralData.Token);
                    logMessage += string.Format("Referrer user id: {0}\n", referralData.ReferrerUserId);
                    logMessage += string.Format("Referrer channel: {0}\n", referralData.ReferrerChannelId);
                    logMessage += string.Format("Is first match: {0}\n", referralData.IsFirstMatch);
                    logMessage += "Custom referral data:\n" + referralData.CustomReferralData.ToDebugString();
                }
                else
                {
                    logMessage += "No referral data retrieved";
                }

                _console.LogD("Referral data: \n" + logMessage);
            },
            error => _console.LogE(string.Format("Failed to get referral data: {0}", error.Message))

        );
    }

    void DrawSendInvites()
    {
        if (_currentInviteChannels.Length == 0) { return; }

        GUILayout.Space(20);
        GUILayout.Label("Send invites", GSStyles.BigLabelText);
        foreach (var channel in _currentInviteChannels)
        {
            DemoGuiUtils.DrawButton(string.Format("Send {0} Invite", channel.Name),
                () => SendInvite(channel.Id),
                style: GSStyles.Button);
        }
    }

    void DrawSendCustomInvites()
    {
        if (_currentInviteChannels.Length == 0) { return; }

        GUILayout.Space(20);
        GUILayout.Label("Send Customized invites", GSStyles.BigLabelText);
        foreach (var channel in _currentInviteChannels)
        {
            DemoGuiUtils.DrawButton(string.Format("Send {0} Invite", channel.Name),
                () => SendCustomInvite(channel.Id),
                style: GSStyles.Button);
        }
    }

    void SendInvite(string channelId)
    {
        _console.LogD(string.Format("Sending {0} invite...", channelId));
        GetSocial.SendInvite(channelId,
            () => _console.LogD("Successfully sent invite"),
            () => _console.LogW("Sending invite cancelled"),
            error => _console.LogE(string.Format("Failed to send invite: {0}", error.Message))
        );
    }

    void SendCustomInvite(string channelId)
    {
        _console.LogD(string.Format("Sending custom {0} invite...", channelId));
        GetSocial.SendInvite(channelId, CustomInviteContent, CustomReferralData,
            () => _console.LogD("Successfully sent invite"),
            () => _console.LogW("Sending invite cancelled"),
            error => _console.LogE(string.Format("Failed to send invite: {0}", error.Message))
        );
    }
}