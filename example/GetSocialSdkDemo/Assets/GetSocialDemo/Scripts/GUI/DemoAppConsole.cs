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
    const int ConsoleButtonSize = 150;
    const string DateFormat = "HH:mm:ss";

    public bool IsVisible { get; private set; }

    static readonly Vector2 ScrollStep = new Vector2(30, 30);

    Vector2 _scrollPos;
    StringBuilder _textBuilder = new StringBuilder();

    public DemoAppConsole Init()
    {
        const int initialConsoleTextCapacity = 1000;
        _textBuilder = new StringBuilder(initialConsoleTextCapacity);
        _textBuilder.AppendLine("Debug Console: All the info about what is happenning in the app will be displayed here\nPress a button in top right corner to toggle the console view\n");
        return this;
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
        _textBuilder.Append(string.Format("<color={0}>", color));
        _textBuilder.Append("[");
        _textBuilder.Append(System.DateTime.Now.ToString(DateFormat));
        _textBuilder.Append("] ");
        _textBuilder.Append(text);
        _textBuilder.Append("\n\n");
        _textBuilder.Append("</color>");
        _scrollPos = new Vector2(int.MaxValue, int.MaxValue);
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
            _scrollPos -= ScrollStep;
        }
        if(GUI.Button(new Rect(10, Screen.height / 2 + ConsoleButtonSize, ConsoleButtonSize, ConsoleButtonSize), "DOWN"))
        {
            _scrollPos += ScrollStep;
        }
        if(GUI.Button(new Rect(10, Screen.height / 2 + 2 * ConsoleButtonSize, ConsoleButtonSize, ConsoleButtonSize), "CLEAR"))
        {
            Clear();
        }
    }

    void DrawScrollView(Rect consoleRect)
    {
        GUILayout.BeginArea(consoleRect);
        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        GUILayout.TextArea(_textBuilder.ToString(), GSStyles.ConsoleText);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private void Clear()
    {
        _textBuilder = new StringBuilder();
    }
}
