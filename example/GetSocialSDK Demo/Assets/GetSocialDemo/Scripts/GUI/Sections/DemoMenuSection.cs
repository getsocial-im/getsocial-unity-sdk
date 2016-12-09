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
using GetSocialSdk.Core;

public abstract class DemoMenuSection : MonoBehaviour
{
    protected GetSocial getSocial;
    protected GetSocialDemoController demoController;
    protected DemoAppConsole console;

    protected abstract string GetTitle();
    protected abstract void InitGuiElements();
    protected abstract void DrawSectionBody();

    private Vector2 scrollPos;

    protected virtual void GoBack()
    {
        GetComponentInParent<GetSocialDemoController>().ShowMainMenu();
    }
    
    protected virtual bool IsBackButtonActive()
    {
        return true;
    }
    
    #region unity_methods
    protected virtual void Start()
    {
        getSocial = GetSocial.Instance;
    }

    private void Update()
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

    private void OnGUI()
    {
        scrollPos = DemoGuiUtils.DrawScrollBodyWrapper(scrollPos, DrawBody);
    }
    #endregion

    public virtual void Initialize(GetSocialDemoController demoController, GetSocial getSocial, DemoAppConsole console)
    {
        this.demoController = demoController;
        this.getSocial = getSocial;
        this.console = console;
        InitGuiElements();
    }

    private void DrawBody()
    {
        GUILayout.BeginVertical();
        GUILayout.Label(GetTitle(), GSStyles.BigLabelText);
        DrawSectionBody();

        GUILayout.Space(10);
        if(IsBackButtonActive())
        {
            DemoGuiUtils.DrawButton("Back", GoBack, style: GSStyles.Button);
        }
        GUILayout.EndVertical();
    }
}
