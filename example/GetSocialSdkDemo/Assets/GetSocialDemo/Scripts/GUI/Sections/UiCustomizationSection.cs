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

#if USE_GETSOCIAL_UI
using System.Collections.Generic;
using System;
using UnityEngine;
using GetSocialSdk.Ui;

public class UiCustomizationSection : DemoMenuSection
{
    protected Dictionary<string, Action> _buttons;

    #region implemented abstract members of DemoMenuSection

    protected override string GetTitle()
    {
        return "UI Customization";
    }

    protected override void InitGuiElements()
    {
        _buttons = new Dictionary<string, Action>
        {
            { "Load Default UI", SetDefaultConfig },
            { "Load Ruzzle UI", LoadRuzzleUI }
        };
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawButtons(_buttons, GSStyles.Button);
    }

    protected void SetDefaultConfig()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        bool result = GetSocialUi.LoadDefaultConfiguration();
        if (result)
        {
            _console.LogD("Successfully loaded default configuration");
        }
        else
        {
            _console.LogE("Failed load default configuration");
        }
    }

    void LoadRuzzleUI()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        bool result = GetSocialUi.LoadConfiguration("ruzzle/ruzzle.json");
        if (result)
        {
            _console.LogD("Successfully loaded default configuration");
        }
        else
        {
            _console.LogE("Failed load default configuration");
        }
    }

    #endregion
}
#endif