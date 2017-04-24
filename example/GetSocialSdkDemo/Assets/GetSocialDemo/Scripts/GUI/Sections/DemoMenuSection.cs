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

public abstract class DemoMenuSection : MonoBehaviour
{
    protected GetSocialDemoController demoController;
    protected DemoAppConsole _console;

    private bool started = false;

    protected abstract string GetTitle();
    protected abstract void DrawSectionBody();

    Vector2 _scrollPos;

    protected virtual void Start()
    {
        started = true;
    }

    protected virtual void InitGuiElements()
    {

    }

    protected virtual void GoBack()
    {
        GetComponentInParent<GetSocialDemoController>().ShowMainMenu();
    }

    protected virtual bool IsBackButtonActive()
    {
        return true;
    }

    #region unity_methods

    public virtual void Update()
    {
        HandleAndroidBackKey();
    }

    void HandleAndroidBackKey()
    {
        if(Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    void OnGUI()
    {
        if (!started)
        {
            return;
        }
        _scrollPos = DemoGuiUtils.DrawScrollBodyWrapper(_scrollPos, DrawBody);
    }
    #endregion

    public virtual void Initialize(GetSocialDemoController demoController, DemoAppConsole console)
    {
        this.demoController = demoController;
        this._console = console;
        InitGuiElements();
    }

    void DrawBody()
    {
        GUILayout.BeginVertical();
        {
            GUILayout.Space(30);

            GUILayout.BeginHorizontal();
            if(IsBackButtonActive())
            {
                DemoGuiUtils.DrawButton("< Back", GoBack, style: GSStyles.Button);
            }
            GUILayout.Label(GetTitle(), GSStyles.BigLabelText);
            GUILayout.EndHorizontal();


            GUILayout.Space(30);
            DrawSectionBody();
        }
        GUILayout.EndVertical();
    }
}
