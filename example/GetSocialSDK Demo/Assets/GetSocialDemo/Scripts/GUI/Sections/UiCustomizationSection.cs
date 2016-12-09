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

using System.Collections.Generic;
using System;
using UnityEngine;

public class UiCustomizationSection : DemoMenuSection
{
    protected Dictionary<string, Action> buttons;

    /// <summary>
    /// For development purposes. 
    /// You can load UI configuration json hosted somewhere on the web (e.g. Dropbox).
    /// Modify this field to avoid entering URL manually each time
    /// </summary>
    private string urlForJson = "https://downloads.getsocial.im/all/default.json";

    #region implemented abstract members of DemoMenuSection
    protected override string GetTitle()
    {
        return "UI Customization";
    }

    protected override void InitGuiElements()
    {
        buttons = new Dictionary<string, Action> 
        {
            { "Load Default UI", SetDefaultConfig },
            { "Load Custom UI from URL", () => LoadConfigFromUrl(urlForJson) }
        };
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawButtons(buttons, GSStyles.Button);
        urlForJson = GUILayout.TextField(urlForJson, GSStyles.TextField);
    }

    protected void SetDefaultConfig()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        getSocial.Configuration.Clear();
        console.LogD("Loaded default configuration", false);
    }

    private void LoadConfigFromUrl(string uri)
    {
        console.LogD("Loading configuration from: " + uri);
        getSocial.Configuration.SetConfiguration(uri);
        console.LogD("Loaded configuration from URL: " + uri);
    }
    #endregion
}
