using System;
using System.Collections.Generic;
using GetSocialSdk.Core;
using GetSocialSdk.Core.Analytics;
using UnityEngine;

namespace GetSocialDemo.Scripts.GUI.Sections
{
    public class CustomAnalyticsEventSection : DemoMenuSection
    {
        
        protected override string GetTitle()
        {
            return "Custom Analytics Events";
        }

        protected override void DrawSectionBody()
        {
            GUILayout.BeginVertical();        
            DemoGuiUtils.DrawButton("Level Completed", () =>
            {
                var properties = new Dictionary<string, string> {{"level", "1"}};
                TrackCustomEvent("level_completed", properties);
            }, true, GSStyles.Button);
            GUILayout.EndVertical();            

            GUILayout.BeginVertical();        
            DemoGuiUtils.DrawButton("Tutorial Completed", () =>
            {
                TrackCustomEvent("tutorial_completed", null);
            }, true, GSStyles.Button);
            GUILayout.EndVertical();            
            
            GUILayout.BeginVertical();        
            DemoGuiUtils.DrawButton("Achievement Unlocked", () =>
            {
                var properties = new Dictionary<string, string>
                {
                    {"achievement", "early_backer"}, 
                    {"item", "car001"}
                };
                TrackCustomEvent("achievement_unlocked", properties);
            }, true, GSStyles.Button);
            GUILayout.EndVertical();            
        }

        private void TrackCustomEvent(string eventName, Dictionary<string, string> eventProperties)
        {
            if (GetSocial.TrackCustomEvent(eventName, eventProperties))
            {
                _console.LogD("Custom event was tracked");
            } else 
            {
                _console.LogD("Failed to track custom event.");
            };
            
        }        
        
    }
}