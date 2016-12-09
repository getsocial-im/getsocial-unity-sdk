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

public class CloudSaveSection : DemoMenuSection
{
    private Dictionary<string, Action> sectionButtons;
    private string stateToSave = "Enter state to save here...";

    #region implemented abstract members of DemoMenuSection
    protected override string GetTitle()
    {
        return "Cloud Save";
    }

    protected override void InitGuiElements()
    {
        sectionButtons = new Dictionary<string, Action> 
        {
            { "Save State", SaveState },
            { "Load State", GetSavedState }
        };
    }

    protected override void DrawSectionBody()
    {
        stateToSave = GUILayout.TextField(stateToSave, GSStyles.TextField);
        DemoGuiUtils.DrawButtons(sectionButtons, GSStyles.Button);
    }
    #endregion

    private void SaveState()
    {
        getSocial.Save(stateToSave,
            () => console.LogD("Successfully saved state"),
            error => console.LogD("Saving state failed: " + error));
    }

    private void GetSavedState()
    {
        getSocial.GetLastSave(
            state => console.LogD("Successfully retrieved state: " + state),
            error => console.LogD("Loading state failed: " + error));
    }
}
