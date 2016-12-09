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

using GetSocialSdk.Core;
using UnityEngine;
using System.Collections.Generic;

public class SmartInvitesSection : DemoMenuSection
{
    public Texture2D imageToShare;

    private enum SubSection
    {
        Main,
        CustomSmartInvites
    }

    private SubSection currentSubSection = SubSection.Main;

    // Custom invites
    private string customSubject = "Custom Subject";
    private string customText = string.Format("It's {0}!, Download it from {1}, I am {2}, app icon is {3}, package is {4}", 
                                    GetSocial.AppNamePlaceholder, 
                                    GetSocial.AppInviteUrlPlaceholder,
                                    GetSocial.UserDisplayNamePlaceholder,
                                    GetSocial.AppIconUrlPlaceholder,
                                    GetSocial.AppPackageNamePlaceholder);

    private string key1 = "key1", key2 = "key2", key3 = "key3";
    private string value1 = "value1", value2 = "value2", value3 = "value3";

    #region implemented abstract members of DemoMenuSection
    protected override string GetTitle()
    {
        if(currentSubSection == SubSection.CustomSmartInvites)
        {
            return "Customize Invites";
        }

        return "Smart Invites";
    }

    protected override void InitGuiElements()
    {
    }

    protected override void DrawSectionBody()
    {
        if(currentSubSection == SubSection.CustomSmartInvites)
        {
            DrawCustomInvitesSubSection();
            return;
        }

        DrawMainSection();
    }
    #endregion

    private void DrawMainSection()
    {
        DemoGuiUtils.DrawButton("Open Smart Invites", CreateSmartInvitesView, style: GSStyles.Button);
        DemoGuiUtils.DrawButton("Send Custom Smart Invite", () => currentSubSection = SubSection.CustomSmartInvites, style: GSStyles.Button);
    }

    private void DrawCustomInvitesSubSection()
    {
        DemoGuiUtils.DrawButton("Open Customized Smart Invites", OpenCustomSmartInvites, style: GSStyles.Button);
        DrawCustomInviteParamsForm();
    }

    private void DrawCustomInviteParamsForm()
    {
        GUILayout.Label("Customize your invite subject and text (optional)", GSStyles.NormalLabelText);

        // Subject
        GUILayout.BeginHorizontal();
        GUILayout.Label("Subject", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        customSubject = GUILayout.TextField(customSubject, GSStyles.TextField, GUILayout.Width(Screen.width * 0.75f));
        GUILayout.EndHorizontal();

        // Text
        GUILayout.BeginHorizontal();
        GUILayout.Label("Text", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        customText = GUILayout.TextField(customText, GSStyles.TextField, GUILayout.Width(Screen.width * 0.75f));
        GUILayout.EndHorizontal();

        // Custom data 
        GUILayout.Label("Attach some key/value pairs to your invite (optional)", GSStyles.NormalLabelText);
        DrawKeyValuePair(ref key1, ref value1);
        DrawKeyValuePair(ref key2, ref value2);
        DrawKeyValuePair(ref key3, ref value3);
    }

    private void DrawKeyValuePair(ref string key, ref string value)
    {
        GUILayout.BeginHorizontal();
        GSStyles.TextField.fixedWidth = Screen.width / 2 - 12f;

        if(string.IsNullOrEmpty(key))
        {
            key = "key";
        }
        if(string.IsNullOrEmpty(value))
        {
            value = "value";
        }
        key = GUILayout.TextField(key, GSStyles.TextField);
        value = GUILayout.TextField(value, GSStyles.TextField);

        GSStyles.TextField.fixedWidth = 0;
        GUILayout.EndHorizontal();
    }

    protected override void GoBack()
    {
        if(currentSubSection == SubSection.CustomSmartInvites)
        {
            currentSubSection = SubSection.Main;
            return;
        }

        base.GoBack();
    }

    private void CreateSmartInvitesView()
    {
        getSocial.CreateSmartInviteView().SetImage(imageToShare).Show();
    }

    private void OpenCustomSmartInvites()
    {
        getSocial.CreateSmartInviteView()
                .SetSubject(customSubject)
                .SetReferralData(GetReferralData())
                .SetText(customText).Show();
    }

    private Dictionary<string, string> GetReferralData()
    {
        return new Dictionary<string, string> 
        {
            { key1, value1 },
            { key2, value2 },
            { key3, value3 }
        };
    }
}
