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
using System;
using System.Collections.Generic;

public static class DemoGuiUtils
{
    public const int HeaderHeight = 140;
    public const int FooterHeight = 40;
    
    public const int Offset = 10;

    public static void DrawHeaderWrapper(Action drawHeaderAction)
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, HeaderHeight), new GUIStyle("box"));
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        
        drawHeaderAction();

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    public static void DrawBodyWrapper(Action drawBodyAction)
    {
        GUILayout.BeginArea(GetBodyRect(), new GUIStyle());

        drawBodyAction();

        GUILayout.EndArea();
    }

    public static Vector2 DrawScrollBodyWrapper(Vector2 scrollPos, Action drawBodyAction)
    {
        GUILayout.BeginArea(GetBodyRect(), new GUIStyle());
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        drawBodyAction();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        return scrollPos;
    }

    public static void DrawFooter(Action drawFooterAction)
    {
        GUILayout.BeginArea(new Rect(0, Screen.height - FooterHeight, Screen.width, FooterHeight), new GUIStyle("box"));
        GUILayout.BeginHorizontal();
        
        drawFooterAction();

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public static int CalcBodyHeight()
    {
        return Screen.height - HeaderHeight - FooterHeight;
    }

    private static Rect GetBodyRect()
    {
        return new Rect(0, HeaderHeight, Screen.width, CalcBodyHeight());
    }

    public static void DrawButton(string name, Action callback, bool isVisible = true, GUIStyle style = null, params GUILayoutOption[] opts)
    {
        GUI.enabled = isVisible;
        if(style == null)
        {
            style = GUI.skin.button;
        }
        if(GUILayout.Button(name, style, opts))
        {
            callback();
        }
        GUI.enabled = true;
    }

    public static void DrawButtons(Dictionary<string, Action> buttons, GUIStyle style = null)
    {
        foreach (var btn in buttons)
        {
            DrawButton(btn.Key, btn.Value, style: style);
        }
    }
    
    public static void Space()
    {
        GUILayout.Space(20f);
    }
}
