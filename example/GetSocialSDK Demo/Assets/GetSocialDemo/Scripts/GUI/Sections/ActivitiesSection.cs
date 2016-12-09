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

public class ActivitiesSection : DemoMenuSection
{
    private enum Section
    {
        Main,
        Post
    }

    private Section activeSection = Section.Main;
    private Dictionary<string, Action> sectionButtons;
    private Dictionary<string, Action> postSubsectionButtons;

    #region implemented abstract members of DemoMenuSection
    protected override void InitGuiElements()
    {
        sectionButtons = new Dictionary<string, Action>
        {
            { "Open Global Activities", OpenGlobalActivities },
            { "Open Activities Filtered by Group", OpenActivitiesFilteredByGroup }
        };
        postSubsectionButtons = new Dictionary<string, Action>
        {
            { "Post Text", PostText },
            { "Post Image", PostImage },
            { "Post Text + Image", () => PostTextAndImage() },
            { "Post Text + Button", () => PostTextAndButton() },
            { "Post Text + Image + Button", () => PostTextImageAndButton() },
            { "Post Image + Button", () => PostImageWithButton() },
            { "Post Image + Action", () => PostImageWithAction() }
        };
    }

    protected override string GetTitle()
    {
        return activeSection == Section.Post ? "Post Activities" : "Activities";
    }

    protected override void DrawSectionBody()
    {
        switch(activeSection)
        {
        case Section.Main:
            DemoGuiUtils.DrawButtons(sectionButtons, GSStyles.Button);
            DemoGuiUtils.DrawButton("Post Activities", () =>
            {
                activeSection = Section.Post;
            }, true, GSStyles.Button);
            break;
        case Section.Post:
            DemoGuiUtils.DrawButtons(postSubsectionButtons, GSStyles.Button);
            break;
        }
    }

    protected override void GoBack()
    {
        if(activeSection == Section.Main)
        {
            base.GoBack();
        }
        else
        {
            activeSection = Section.Main;
        }
    }
    #endregion

    private void OpenGlobalActivities()
    {
        getSocial.CreateActivitiesView().SetTitle("Global Activities").Show();
    }

    private void OpenActivitiesFilteredByGroup()
    {
        var customGroup = "specials";
        var tags = new [] { "level1", "nl" };
        getSocial.CreateActivitiesView(customGroup, tags).SetTitle("Specials").Show();
    }

    private void PostText()
    {
        getSocial.PostActivity("Text", null, null, null, null, data => OpenGlobalActivities(), LogPostingFailed);
    }

    private void PostImage()
    {
        getSocial.PostActivity(null, Resources.Load<Texture2D>("activityImage").EncodeToPNG(), null, null, null, data => OpenGlobalActivities(), LogPostingFailed);
    }

    private void PostTextAndImage()
    {
        getSocial.PostActivity("Text+Image", Resources.Load<Texture2D>("activityImage").EncodeToPNG(), null, null, null, data => OpenGlobalActivities(), LogPostingFailed);
    }

    private void PostTextAndButton()
    {
        getSocial.PostActivity("Text+Button", null, "Click here", "action", null, (data) => OpenGlobalActivities(), LogPostingFailed);
    }

    private void PostTextImageAndButton()
    {
        getSocial.PostActivity("Text+Image+Button", Resources.Load<Texture2D>("activityImageWithAction").EncodeToPNG(), "Click here", "action", null, data => OpenGlobalActivities(), LogPostingFailed);
    }

    private void PostImageWithAction()
    {
        getSocial.PostActivity(null, Resources.Load<Texture2D>("activityImageWithAction").EncodeToPNG(), "Click here", "action", null, data => OpenGlobalActivities(), LogPostingFailed);
    }

    private void PostImageWithButton()
    {
        getSocial.PostActivity(null, Resources.Load<Texture2D>("activityImageWithButton").EncodeToPNG(), null, "action", null, data => OpenGlobalActivities(), LogPostingFailed);
    }

    private void LogPostingFailed()
    {
        console.LogE("Posting activity failed");
    }
}
