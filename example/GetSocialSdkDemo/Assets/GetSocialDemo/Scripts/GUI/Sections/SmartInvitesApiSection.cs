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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Assets.GetSocialDemo.Scripts.Utils;
using GetSocialSdk.Core;
using GetSocialSdk.Ui;
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

    public Dictionary<string, object> CustomLinkParams
    {
        get
        {
            var linkParams = new Dictionary<string, object>
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
                linkParams[LinkParams.KeyCustomYoutubeVideo] = _customLandingPageVideoURL;
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
            var mediaAttachment = _sendCustomImage ? MediaAttachment.WithImage(Image)
                : _sendCustomVideo ? MediaAttachment.WithVideo(Video)
                : null;

            var inviteContent = new InviteContent
            {
                Subject = _customWindowTitle,
                Text = _customText,
                MediaAttachment = mediaAttachment,
            }.AddLinkParams(CustomLinkParams);
            return inviteContent;
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
        DemoGuiUtils.DrawButton("Open Smart Invites", ShowNativeSmartInvitesView, style: GSStyles.Button);
        DemoGuiUtils.DrawButton("Open Customized Smart Invites", ShowNativeSmartInvitesViewCustomized, style: GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Available Channels Details", PrintAvailableInviteChannelsDetails, style: GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Available Channels", PrintAvailableChannelsList, style: GSStyles.Button);
        DrawCreateInviteLink();
        DrawCustomInviteParamsForm();

        // API calls to SendInvite
        DrawSendInvites();
        DrawSendCustomInvites();
   }
    
    static void ShowNativeSmartInvitesView()
    {
        InvitesViewBuilder.Create().Show();
    }

    void ShowNativeSmartInvitesViewCustomized()
    {
        var inviteContent = CustomInviteContent;
        InvitesViewBuilder.Create()
            .SetWindowTitle(CustomTitle)
            .SetCustomInviteContent(CustomInviteContent)
            .SetInviteCallbacks(
                channelId => _console.LogD("Successfully sent invite for " + channelId),
                channelId => _console.LogW("Sending invite cancelled for " + channelId),
                (channelId, error) => _console.LogE(string.Format("Failed to send invite: {0} for {1}", error.Message, channelId)))
            .Show();
    }

    void PrintAvailableInviteChannelsDetails()
    {
        Invites.GetAvailableChannels((inviteChannels) => {
            _currentInviteChannels = inviteChannels.ToArray();
            var channels = _currentInviteChannels.ToList().ConvertAll(x => x.ToString()).ToArray();
            var channelsJoined = string.Join(", ", channels);
            _console.LogD(string.Format("Available invite channels: {0}", channelsJoined));
        }, (error) => {
            _console.LogE("Error while getting invite channels: " + error.Message);
        });
    }
    
    private void PrintAvailableChannelsList()
    {

        Invites.GetAvailableChannels((availableChannels) => {
            var messageBuilder = new StringBuilder("Invite channels availability:\n");
            availableChannels.ForEach(channel => messageBuilder.AppendLine(channel.Id));
            _console.LogD(messageBuilder.ToString());
        }, (error) => {
            _console.LogE("failed to get available invite channels, error: " + error);
        });
        
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
        Invites.Send(null, channelId,
            () => _console.LogD("Successfully sent invite"),
            () => _console.LogW("Sending invite cancelled"),
            error => _console.LogE(string.Format("Failed to send invite: {0}", error.Message))
        );
    }

    void CreateInviteLink()
    {
        _console.LogD("Creating invite link...");
        Invites.CreateLink(CustomLinkParams, 
            (String invite) => _console.LogD("Created invite link: " + invite),
            error => _console.LogE(string.Format("Failed to create invite: {0}", error.Message))
        );
    }

    void SendCustomInvite(string channelId)
    {
        _console.LogD(string.Format("Sending custom {0} invite...", channelId));
        Invites.Send(CustomInviteContent, channelId,
            () => _console.LogD("Successfully sent invite"),
            () => _console.LogW("Sending invite cancelled"),
            error => _console.LogE(string.Format("Failed to send invite: {0}", error.Message))
        );
    }
}