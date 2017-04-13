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
    protected enum SettingsSubSection
    {
        Main,
        ChooseLanguage
    }

    protected SettingsSubSection _currentSection = SettingsSubSection.Main;

    static readonly string[] _supportedLanguagesRow1 = {"da", "de", "en", "es"};
    static readonly string[] _supportedLanguagesRow2 = {"fr", "it", "nb", "nl"};
    static readonly string[] _supportedLanguagesRow3 = {"pt", "ru", "sv", "tr"};
    static readonly string[] _supportedLanguagesRow4 = {"is", "ja", "ko", "zh-Hans"};
    static readonly string[] _supportedLanguagesRow5 = {"zh-Hant", "id", "tl", "ms"};
    static readonly string[] _supportedLanguagesRow6 = {"pt-br", "vi", "uk", "pl"};
    readonly Dictionary<string, Action> _languageButtonsRow1 = new Dictionary<string, Action>();
    readonly Dictionary<string, Action> _languageButtonsRow2 = new Dictionary<string, Action>();
    readonly Dictionary<string, Action> _languageButtonsRow3 = new Dictionary<string, Action>();
    readonly Dictionary<string, Action> _languageButtonsRow4 = new Dictionary<string, Action>();
    readonly Dictionary<string, Action> _languageButtonsRow5 = new Dictionary<string, Action>();
    readonly Dictionary<string, Action> _languageButtonsRow6 = new Dictionary<string, Action>();

    protected override string GetTitle()
    {
        return _currentSection == SettingsSubSection.ChooseLanguage ? "Change Language" : "Settings";
    }

    protected override void InitGuiElements()
    {
        InitLanguageButtonsRow(_languageButtonsRow1, _supportedLanguagesRow1);
        InitLanguageButtonsRow(_languageButtonsRow2, _supportedLanguagesRow2);
        InitLanguageButtonsRow(_languageButtonsRow3, _supportedLanguagesRow3);
        InitLanguageButtonsRow(_languageButtonsRow4, _supportedLanguagesRow4);
        InitLanguageButtonsRow(_languageButtonsRow5, _supportedLanguagesRow5);
        InitLanguageButtonsRow(_languageButtonsRow6, _supportedLanguagesRow6);
    }

    void InitLanguageButtonsRow(Dictionary<string, Action> buttons, string[] langCodes)
    {
        foreach (var langCode in langCodes)
        {
            var code = langCode;
            buttons[langCode] = () => SetLanguage(code);
        }
    }

    protected override void DrawSectionBody()
    {
        if (_currentSection == SettingsSubSection.ChooseLanguage)
        {
            DrawChooseLanguageSubsection();
            return;
        }

        DrawMainSection();
    }

    void DrawMainSection()
    {
        DemoGuiUtils.DrawButton("Change Language", () => _currentSection = SettingsSubSection.ChooseLanguage,
            style: GSStyles.Button);
        DemoGuiUtils.DrawButton("Set Global Error Listener", () =>
            {
                var result = GetSocial.SetGlobalErrorListener(OnGlobalError);
                if (result)
                {
                    _console.LogD("Successfully set global error listener");
                }
                else
                {
                    _console.LogE("Failed to set global error listener");
                }
            },
            style: GSStyles.Button);
        DemoGuiUtils.DrawButton("Remove Global Error Listener", () =>
            {
                var result = GetSocial.RemoveGlobalErrorListener();
                if (result)
                {
                    _console.LogD("Successfully removed global error listener");
                }
                else
                {
                    _console.LogE("Failed to remove global error listener");
                }
            },
            style: GSStyles.Button);
    }

    void OnGlobalError(GetSocialError ex)
    {
        _console.LogD(string.Format("Global error: {0}", ex.Message));
    }

    protected override void GoBack()
    {
        if (_currentSection == SettingsSubSection.Main)
        {
            base.GoBack();
        }
        else
        {
            _currentSection = SettingsSubSection.Main;
        }
    }

    #region set_language

    void DrawChooseLanguageSubsection()
    {
        DemoGuiUtils.DrawButton("Get Current Language",
            () => { _console.LogD(string.Format("Current lang is '{0}'", GetSocial.GetLanguage())); },
            style: GSStyles.Button);
        DemoGuiUtils.DrawButton("Try Set Incorrect Language",
            () => { SetLanguage("INCORRECT_LNAG_CODE"); },
            style: GSStyles.Button);
        GUILayout.Space(10f);
        GSStyles.Button.fixedWidth = Screen.width / 4 - 5f;
        DrawLanguagesRow(_languageButtonsRow1);
        DrawLanguagesRow(_languageButtonsRow2);
        DrawLanguagesRow(_languageButtonsRow3);
        DrawLanguagesRow(_languageButtonsRow4);
        DrawLanguagesRow(_languageButtonsRow5);
		DrawLanguagesRow(_languageButtonsRow6);
        GSStyles.Button.fixedWidth = 0;
        GUILayout.Space(10f);
    }

    static void DrawLanguagesRow(Dictionary<string, Action> buttons)
    {
        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButtons(buttons, GSStyles.Button);
        GUILayout.EndHorizontal();
    }

    void SetLanguage(string languageCode)
    {
        var result = GetSocial.SetLanguage(languageCode);
        _console.LogD(result
            ? string.Format("Successfully set language to '{0}", GetSocial.GetLanguage())
            : string.Format("Failed set language, current is '{0}'", GetSocial.GetLanguage()));
    }

    #endregion
}