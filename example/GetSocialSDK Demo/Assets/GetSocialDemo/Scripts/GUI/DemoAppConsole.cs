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
using System.Text;

public class DemoAppConsole : MonoBehaviour
{
    private const int ConsoleButtonSize = 150;
    private const string DateFormat = "HH:mm:ss";

    public bool IsVisible { get; private set; }

    private static readonly Vector2 ScrollStep = new Vector2(30, 30);

    private Vector2 scrollPos;
    private StringBuilder textBuilder;

    void Awake()
    {
        const int initialConsoleTextCapacity = 1000;
        textBuilder = new StringBuilder(initialConsoleTextCapacity);
        textBuilder.AppendLine("Debug Console: All the info about what is happenning in the app will be displayed here\nPress a button in top right corner to toggle the console view\n");
    }

    public void Toggle()
    {
        IsVisible = !IsVisible;
    }

    #region logging
    public void LogD(string text, bool showImmediately = true)
    {
        Log(text, "lime", showImmediately);
        Debug.Log(text);
    }

    public void LogW(string text, bool showImmediately = true)
    {
        Log(text, "yellow", showImmediately);
        Debug.LogWarning(text);
    }

    public void LogE(string text, bool showImmediately = true)
    {
        Log(text, "red", showImmediately);
        Debug.LogError(text);
    }

    private void Log(string text, string color, bool showImmediately = true)
    {
        IsVisible |= showImmediately;
        textBuilder.Append(string.Format("<color={0}>", color));
        textBuilder.Append("[");
        textBuilder.Append(System.DateTime.Now.ToString(DateFormat));
        textBuilder.Append("] ");
        textBuilder.Append(text);
        textBuilder.Append("\n\n");
        textBuilder.Append("</color>");
        scrollPos = new Vector2(int.MaxValue, int.MaxValue);
    }
    #endregion

    private void OnGUI()
    {
        if(!IsVisible)
        {
            return;
        }

        Rect consoleRect = new Rect(0, DemoGuiUtils.HeaderHeight, Screen.width, DemoGuiUtils.CalcBodyHeight());
        GUI.Box(consoleRect, string.Empty, GSStyles.ConsoleBg);

        DrawNavigation();

        DrawScrollView(consoleRect);
    }

    private void DrawNavigation()
    {
        if(GUI.Button(new Rect(10, Screen.height / 2, ConsoleButtonSize, ConsoleButtonSize), "UP"))
        {
            scrollPos -= ScrollStep;
        }
        if(GUI.Button(new Rect(10, Screen.height / 2 + ConsoleButtonSize, ConsoleButtonSize, ConsoleButtonSize), "DOWN"))
        {
            scrollPos += ScrollStep;
        }
        if(GUI.Button(new Rect(10, Screen.height / 2 + 2 * ConsoleButtonSize, ConsoleButtonSize, ConsoleButtonSize), "CLEAR"))
        {
            Clear();
        }
    }

    void DrawScrollView(Rect consoleRect)
    {
        GUILayout.BeginArea(consoleRect);
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.TextArea(textBuilder.ToString(), GSStyles.ConsoleText);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private void Clear()
    {
        textBuilder = new StringBuilder();
    }
}
