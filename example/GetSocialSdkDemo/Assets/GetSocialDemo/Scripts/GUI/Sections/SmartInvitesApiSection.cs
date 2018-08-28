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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Assets.GetSocialDemo.Scripts.Utils;
using GetSocialSdk.Core;
using UnityEngine;

public class SmartInvitesApiSection : DemoMenuSection
{
    // Custom invites
    string _customWindowTitle = "Custom Subject";

    string _customText = string.Format("It's an awesome app!, Download it from {0}",
        InviteTextPlaceholders.PlaceholderAppInviteUrl);

    string _key1 = "key1", _key2 = "key2", _key3 = "key3";
    string _value1 = "value1", _value2 = "value2", _value3 = "value3";

    Texture2D _image;

    Texture2D Image
    {
        get { return _image ?? (_image = Resources.Load<Texture2D>("activityImage")); }
    }
    
    // Custom Link Params
    string _customLandingPageTitle = "Custom Landing Page Title";

    string _customLandingPageDescription = "Custom Landing Page Description";
    
    string _customLandingPageImageURL = "";

    string _customLandingPageVideoURL = "";

    private bool _useInviteImage;
    private bool _useCustomImage;

    private bool _sendCustomImage;
    private bool _sendCustomVideo;

    private Texture2D CustomLandingPageImage
    {
        get
        {
            if (_useCustomImage)
            {
                return Resources.Load<Texture2D>("landingPage");
            }
            if (_useInviteImage)
            {
                return Image;
            }
            return null;
            
        }
    }
    
    byte[] _video;

    private byte[] Video
    {
        get
        {
            if (_video == null)
            {
                _video = DemoUtils.LoadSampleVideoBytes();
            }
            return _video;
        }
    }


    InviteChannel[] _currentInviteChannels = { };

    public string CustomTitle
    {
        get { return _customWindowTitle; }
    }

    public LinkParams LinkParams
    {
        get
        {
            LinkParams linkParams = new LinkParams
            {
                {_key1, _value1},
                {_key2, _value2},
                {_key3, _value3}
            };
            if (_customLandingPageTitle.Length > 0)
            {
                linkParams[LinkParams.KeyCustomTitle] =  _customLandingPageTitle;
            }
            if (_customLandingPageDescription.Length > 0)
            {
                linkParams[LinkParams.KeyCustomDescription] = _customLandingPageDescription;
            }
            if (_customLandingPageImageURL.Length > 0)
            {
                linkParams[LinkParams.KeyCustomImage] = _customLandingPageImageURL;
            }
            if (_customLandingPageVideoURL.Length > 0)
            {
                linkParams[LinkParams.KeyCustomYouTubeVideo] = _customLandingPageVideoURL;
            }
            if (CustomLandingPageImage != null)
            {
                linkParams[LinkParams.KeyCustomImage] = CustomLandingPageImage;
            }
            return linkParams;
        }
    }

    public InviteContent CustomInviteContent
    {
        get
        {
            InviteContent.Builder builder = InviteContent.CreateBuilder();
            builder.WithSubject(_customWindowTitle);
            builder.WithText(_customText);
            if (_sendCustomImage)
            {
                builder.WithImage(Image);
            }
            if (_sendCustomVideo)
            {
                builder.WithVideo(Video);
            }

            return builder.Build();
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
        DemoGuiUtils.DrawButton("Get Available Channels Details", PrintAvailableInviteChannelsDetails, style: GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Available Channels", PrintAvailbleChannelsList, style: GSStyles.Button);
        DrawCreateInviteLink();
        DrawCustomInviteParamsForm();

        // API calls to SendInvite
        DrawReferralData();
        DrawSendInvites();
        DrawSendCustomInvites();
        DrawGetReferredUsers();
    }

    void PrintAvailableInviteChannelsDetails()
    {
        _currentInviteChannels = GetSocial.InviteChannels;
        var channels = _currentInviteChannels.ToList().ConvertAll(x => x.ToString()).ToArray();
        var channelsJoined = string.Join(", ", channels);
        _console.LogD(string.Format("Available invite channels: {0}", channelsJoined));
    }
    
    private void PrintAvailbleChannelsList()
    {
        var channelIds = typeof(InviteChannelIds)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
            .Select(field => field.GetValue("").ToString())
            .ToList();

        var messageBuilder = new StringBuilder("Invite channels availability:\n");
        channelIds.ForEach(channelId => messageBuilder.AppendLine(string.Format("{0}: {1}", channelId, GetSocial.IsInviteChannelAvailable(channelId))));
        
        _console.LogD(messageBuilder.ToString());
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

        // Image/Video
        GUILayout.BeginHorizontal();
        GUILayout.Label("Image/Video", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));

        if (GUILayout.Toggle(_sendCustomImage, "", GSStyles.ImageToggle))
        {
            _sendCustomImage = true; 
            _sendCustomVideo = false;
        }
        GUILayout.Label("Send Custom Image", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));

        if (GUILayout.Toggle(_sendCustomVideo, "", GSStyles.ImageToggle))
        {
            _sendCustomImage = false;
            _sendCustomVideo = true;
        }
        GUILayout.Label("Send Custom Video", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        
        if (GUILayout.Button("Clear", GSStyles.ClearButton))
        {
            _sendCustomImage = false;
            _sendCustomVideo = false;
        }
        
        GUILayout.EndHorizontal();
          
        // Link Params
        GUILayout.Label("Customize landing page (optional)", GSStyles.NormalLabelText);

        // Title
        GUILayout.BeginHorizontal();
        GUILayout.Label("Title", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        _customLandingPageTitle = GUILayout.TextField(_customLandingPageTitle, GSStyles.TextField,
            GUILayout.Width(Screen.width * 0.75f));
        GUILayout.EndHorizontal();

        // Description
        GUILayout.BeginHorizontal();
        GUILayout.Label("Description", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        _customLandingPageDescription = GUILayout.TextField(_customLandingPageDescription, GSStyles.TextField,
            GUILayout.Width(Screen.width * 0.75f));
        GUILayout.EndHorizontal();

        // Image URL
        GUILayout.BeginHorizontal();
        GUILayout.Label("Image URL", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        _customLandingPageImageURL = GUILayout.TextField(_customLandingPageImageURL, GSStyles.TextField,
            GUILayout.Width(Screen.width * 0.75f));
        GUILayout.EndHorizontal();

        // Video URL
        GUILayout.BeginHorizontal();
        GUILayout.Label("Video URL", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        _customLandingPageVideoURL = GUILayout.TextField(_customLandingPageVideoURL, GSStyles.TextField,
            GUILayout.Width(Screen.width * 0.75f));
        GUILayout.EndHorizontal();

        // Image
        GUILayout.BeginHorizontal();
        GUILayout.Label("Image", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));

        if (GUILayout.Toggle(_useCustomImage, "", new GUIStyle(GUI.skin.toggle)
        {
            name = "toggleCustomImage",
            padding = new RectOffset(40, 0, 40, 0),
            border = new RectOffset(0, 0, 0, 0),
            overflow = new RectOffset(0, 0, 0, 0),
            imagePosition = ImagePosition.ImageOnly,
            stretchHeight = false,
            stretchWidth = false
        }))
        {
            _useCustomImage = true; 
            _useInviteImage = false;
        }
        GUILayout.Label("Use Custom Image", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));

        if (GUILayout.Toggle(_useInviteImage, "", new GUIStyle(GUI.skin.toggle)
        {
            name = "toggleInviteImage",
            padding = new RectOffset(40, 0, 40, 0),
            border = new RectOffset(0, 0, 0, 0),
            overflow = new RectOffset(0, 0, 0, 0),
            imagePosition = ImagePosition.ImageOnly,
            stretchHeight = false,
            stretchWidth = false
        }))
        {
            _useCustomImage = false;
            _useInviteImage = true;
        }
        GUILayout.Label("Use Invite Image", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        
        if (GUILayout.Button("Clear", new GUIStyle(GUI.skin.button)
        {
            fontSize = 16,
            fixedHeight = 40,
            stretchWidth = true,
            richText = false,
            alignment = TextAnchor.MiddleCenter
        }))
        {
            _useCustomImage = false;
            _useInviteImage = false;
        }

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
    
    private void DrawGetReferredUsers()
    {
        GUILayout.Label("Referred users", GSStyles.BigLabelText);

        DemoGuiUtils.DrawButton("Get referred users", GetReferredUsers, style: GSStyles.Button);
    }

    private void GetReferredUsers()
    {
        GetSocial.GetReferredUsers(referredUsers =>
            {
                var message = "";
                if (referredUsers.Count > 0)
                {
                    foreach (var referredUser in referredUsers)
                    {
                        message += string.Format("{0}(on {1:yy-MM-dd HH:mm} via {2}), ", referredUser.DisplayName, referredUser.InstallationDate, referredUser.InstallationChannel);
                    }
                    message = message.Substring(0, message.Length - 2);
                }
                else
                {
                    message = "No referred user found.";
                }
                DemoUtils.ShowPopup("Referred Users", message);
            }, 
            error => _console.LogE(string.Format("Failed to get referred users: {0}", error.Message)));
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
                    logMessage += string.Format("Is guarateed match: {0}\n", referralData.IsGuaranteedMatch);
                    logMessage += "Link params:\n" + referralData.LinkParams.ToDebugString();
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
    
    void DrawCreateInviteLink()
    {

        GUILayout.Space(20);
        DemoGuiUtils.DrawButton("Create Invite Link",
            () => CreateInviteLink(),
            style: GSStyles.Button);
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

    void CreateInviteLink()
    {
        _console.LogD("Creating invite link...");
        GetSocial.CreateInviteLink(null, 
            (string inviteLink) => _console.LogD("Created invite link: " + inviteLink),
            error => _console.LogE(string.Format("Failed to create invite link: {0}", error.Message))
        );
    }

    void SendCustomInvite(string channelId)
    {
        _console.LogD(string.Format("Sending custom {0} invite...", channelId));
        GetSocial.SendInvite(channelId, CustomInviteContent, LinkParams,
            () => _console.LogD("Successfully sent invite"),
            () => _console.LogW("Sending invite cancelled"),
            error => _console.LogE(string.Format("Failed to send invite: {0}", error.Message))
        );
    }
}