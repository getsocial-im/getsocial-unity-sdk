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
using System.Collections.Generic;
using System;
using GetSocialSdk.Core;

public class SettingsSection : DemoMenuSection
{
    private enum SettingsSubSection
    {
        Main,
        ChooseLanguage
    }

    private SettingsSubSection currentSection = SettingsSubSection.Main;

    private static readonly string[] supportedLanguagesRow1 = {"da", "de", "en", "es"};
    private static readonly string[] supportedLanguagesRow2 = {"fr", "it", "nb", "nl"};
    private static readonly string[] supportedLanguagesRow3 = {"pl", "pt", "ru", "sv"};
    private static readonly string[] supportedLanguagesRow4 = {"tr", "is", "ja", "ko"};
    private static readonly string[] supportedLanguagesRow5 = {"zh-Hans", "zh-Hant", "id", "tl"};
    private static readonly string[] supportedLanguagesRow6 = {"pt-br", "vi", "ms", "uk"};
    private Dictionary<string, Action> languageButtonsRow1 = new Dictionary<string, Action>();
    private Dictionary<string, Action> languageButtonsRow2 = new Dictionary<string, Action>();
    private Dictionary<string, Action> languageButtonsRow3 = new Dictionary<string, Action>();
    private Dictionary<string, Action> languageButtonsRow4 = new Dictionary<string, Action>();
    private Dictionary<string, Action> languageButtonsRow5 = new Dictionary<string, Action>();
    private Dictionary<string, Action> languageButtonsRow6 = new Dictionary<string, Action>();

    #region checkboxes
    private bool isAnonymousPostingForbidden = false;
    private bool isContentModerated = false;
    private bool isUserAvatarClickHandlerCustom = false;
    private bool isAppAvatarClickHandlerCustom = false;
    private bool isActivityActionHandlerCustom = false;
    private bool isInviteButtonClickHandlerCustom = false;
    private bool isOnUserPerformedActionHandlerCustom = false;
    #endregion

    protected override void Start()
    {
        base.Start();
        SetCustomBehaviorListeners();
    }

    public override void Initialize(GetSocialDemoController demoController, GetSocial getSocial, DemoAppConsole console)
    {
        base.Initialize(demoController, getSocial, console);
        SetCustomBehaviorListeners();
    }

    #region listeners
    private void SetCustomBehaviorListeners()
    {
        EnableContentModeration();
        EnableCustomUserAvatarClick();
        EnableCustomAppAvatarClick();
        EnableCustomActivityActionHandler();
        EnableCustomInviteButtonClickBehavior();
        EnableCustomOnUserPerformedActionBehavior();
    }

    private void EnableContentModeration()
    {
        getSocial.SetOnUserGeneratedContentListener((contentSource, content) => isContentModerated ? ModerateUserGeneratedContent(contentSource, content) : content);
    }

    private void EnableCustomUserAvatarClick()
    {
        getSocial.SetOnUserAvatarClickListener((user, source) => 
        {
            if(isUserAvatarClickHandlerCustom)
            {
                console.LogD(string.Format("Clicked on user: {0}", user));
                FollowUnfollowSection.LastSelectedUser = user;
                getSocial.CloseView();
                return true;
            }
            return false;
        });
    }

    private void EnableCustomAppAvatarClick()
    {
        getSocial.SetOnAppAvatarClickListener(() => 
        {
            if(isAppAvatarClickHandlerCustom)
            {
                console.LogD("Custom app avatar click behavior enabled");
                getSocial.CloseView();
                return true;
            }
            return false;
        });
    }

    private void EnableCustomActivityActionHandler()
    {
        getSocial.SetOnActivityActionClickListener(actionId => 
        {
            if(isActivityActionHandlerCustom)
            {
                console.LogD("Custom activity action click behavior enabled: " + actionId);
                getSocial.CloseView();
            }
        });
    }

    private void EnableCustomInviteButtonClickBehavior()
    {
        getSocial.SetOnInviteButtonClickListener(() => 
        {
            if(isInviteButtonClickHandlerCustom)
            {
                console.LogD("Custom invite button click behavior enabled");
                getSocial.CloseView();
                return true;
            }
            return false;
        });
    }

    private void EnableCustomOnUserPerformedActionBehavior()
    {
        getSocial.SetOnUserPerformedActionListener(OnUserActionPerformed);
    }

    private void OnUserActionPerformed(GetSocial.UserPerformedAction action, Action<bool> finalizeAction)
    {
        bool allowAction = true;
        if(isOnUserPerformedActionHandlerCustom)
        {
            console.LogD(string.Format("User has performed [{0}] action", action));
        }

        // forbid anonymous users to perform certain actions
        if(isAnonymousPostingForbidden && IsNonAnonymousUserAction(action) && getSocial.CurrentUser.IsAnonymous)
        {
            console.LogD(action + " is not allowed to be performed by the anonymous user. Please add any of the identities to continue");
            getSocial.CloseView();
            allowAction = false;
        }

        // decide if we allow the user to perform the action and invoke finalize action with our decision
        finalizeAction(allowAction);
    }
    #endregion

    #region implemented abstract members of DemoMenuSection
    protected override string GetTitle()
    {
        return currentSection == SettingsSubSection.ChooseLanguage ? "Change Language" : "Settings";
    }

    protected override void InitGuiElements()
    {
        InitLanguageButtonsRow(languageButtonsRow1, supportedLanguagesRow1);
        InitLanguageButtonsRow(languageButtonsRow2, supportedLanguagesRow2);
        InitLanguageButtonsRow(languageButtonsRow3, supportedLanguagesRow3);
        InitLanguageButtonsRow(languageButtonsRow4, supportedLanguagesRow4);
        InitLanguageButtonsRow(languageButtonsRow5, supportedLanguagesRow5);
        InitLanguageButtonsRow(languageButtonsRow6, supportedLanguagesRow6);
    }

    private void InitLanguageButtonsRow(Dictionary<string, Action> buttons, string[] langCodes)
    {
        for(int i = 0; i < langCodes.Length; i++)
        {
            string langCode = langCodes[i];
            buttons[langCode] = () => SetLanguage(langCode);
        }
    }

    protected override void DrawSectionBody()
    {
        if(currentSection == SettingsSubSection.ChooseLanguage)
        {
            DrawChooseLanguageSubsection();
            return;
        }

        DrawMainSection();
    }

    private void DrawMainSection()
    {
        DemoGuiUtils.DrawButton("Change Language", () => currentSection = SettingsSubSection.ChooseLanguage, style: GSStyles.Button);
        DrawToggle(ref isAnonymousPostingForbidden, "Prevent anonymous users from posting");
        DrawToggle(ref isOnUserPerformedActionHandlerCustom, "Log to console when user performs an action");
        DrawToggle(ref isContentModerated, "Enable user generated content handler");
        DrawToggle(ref isUserAvatarClickHandlerCustom, "User avatar click custom behaviour");
        DrawToggle(ref isAppAvatarClickHandlerCustom, "App avatar click custom behaviour");
        DrawToggle(ref isActivityActionHandlerCustom, "Activity action click custom behaviour");
        DrawToggle(ref isInviteButtonClickHandlerCustom, "Invite button click custom behaviour");
    }

    protected override void GoBack()
    {
        if(currentSection == SettingsSubSection.Main)
        {
            base.GoBack();
        }
        else
        {
            currentSection = SettingsSubSection.Main;
        }
    }
    #endregion

    #region set_language
    private void DrawChooseLanguageSubsection()
    {
        GSStyles.Button.fixedWidth = Screen.width / 4 - 5f;
        DrawLanguagesRow(languageButtonsRow1);
        DrawLanguagesRow(languageButtonsRow2);
        DrawLanguagesRow(languageButtonsRow3);
        DrawLanguagesRow(languageButtonsRow4);
        DrawLanguagesRow(languageButtonsRow5);
        DrawLanguagesRow(languageButtonsRow6);
        GSStyles.Button.fixedWidth = 0;
        GUILayout.Space(10f);
    }

    private void DrawLanguagesRow(Dictionary<string, Action> buttons)
    {
        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButtons(buttons, GSStyles.Button);
        GUILayout.EndHorizontal();
    }

    private void SetLanguage(string languageCode)
    {
        getSocial.SetLanguage(languageCode);
    }
    #endregion

    private void DrawToggle(ref bool setting, string labelText)
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        setting = GUILayout.Toggle(setting, string.Empty, GSStyles.Toggle);
        GUILayout.Label(labelText, GSStyles.NormalLabelText);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    private static String ModerateUserGeneratedContent(GetSocial.ContentSource source, String content)
    {
        var msg = content + " (verified)";
        switch(source)
        {
            case GetSocial.ContentSource.Activity:
                msg = "User is posting an activity: " + msg;
                break;
            case GetSocial.ContentSource.Comment:
                msg = "User is commenting an activity: " + msg;
                break;
            case GetSocial.ContentSource.PrivateChatMessage:
                msg = "User is sending private chat message: " + msg;
                break;
            case GetSocial.ContentSource.PublicChatMessage:
                msg = "User is sending public chat message: " + msg;
                break;
        }

        return msg;
    }

    private static bool IsNonAnonymousUserAction(GetSocial.UserPerformedAction action)
    {
        bool nonAnonymousUserAction = false;
        
        switch(action)
        {
            case GetSocial.UserPerformedAction.LikeActivity:
            case GetSocial.UserPerformedAction.LikeComment:
            case GetSocial.UserPerformedAction.PostActivity:
            case GetSocial.UserPerformedAction.PostComment:
            case GetSocial.UserPerformedAction.SendPrivateChatMessage:
            case GetSocial.UserPerformedAction.SendPublicChatMessage:
                nonAnonymousUserAction = true;
                break;
        }
        
        return nonAnonymousUserAction;
    }
}
