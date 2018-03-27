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

using UnityEngine;

#if USE_GETSOCIAL_UI
using GetSocialSdk.Ui;

public class SmartInvitesUiSection : DemoMenuSection
{
    [SerializeField] SmartInvitesApiSection _smartInvitesApiSection;

    #region implemented abstract members of DemoMenuSection

    protected override string GetTitle()
    {
        return "Smart Invites UI";
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawButton("Open Smart Invites", ShowNativeSmartInvitesView, style: GSStyles.Button);
        DemoGuiUtils.DrawButton("Open Customized Smart Invites", ShowNativeSmartInvitesViewCustomized, style: GSStyles.Button);
        GUILayout.Space(20);
        _smartInvitesApiSection.DrawCustomInviteParamsForm();
    }

    #endregion

    static void ShowNativeSmartInvitesView()
    {
        GetSocialUi.CreateInvitesView().Show();
    }

    void ShowNativeSmartInvitesViewCustomized()
    {
        GetSocialUi.CreateInvitesView()
            .SetWindowTitle(_smartInvitesApiSection.CustomTitle)
            .SetLinkParams(_smartInvitesApiSection.LinkParams)
            .SetCustomInviteContent(_smartInvitesApiSection.CustomInviteContent)
            .SetInviteCallbacks(
                channelId => _console.LogD("Successfully sent invite for " + channelId),
                channelId => _console.LogW("Sending invite cancelled for " + channelId),
                (channelId, error) => _console.LogE(string.Format("Failed to send invite: {0} for {1}", error.Message, channelId)))
            .Show();
    }
}
#endif