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

using UnityEditor;
using UnityEngine;

namespace GetSocialSdk.Editor
{
    public static class EditorGuiUtils
    {
        public static void SelectableLabelField(GUIContent label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
            EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
            EditorGUILayout.EndHorizontal();
        }

        public static GUIContent GetBoldLabel(string text, string tooltip = "", Texture2D icon = null)
        {
            var label = icon != null ? new GUIContent(text, icon, tooltip) : new GUIContent(text, tooltip);
            var style = EditorStyles.foldout;
            style.fontStyle = FontStyle.Bold;
            return label;
        }
    }
}
