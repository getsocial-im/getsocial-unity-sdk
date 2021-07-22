using System;
using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.Core;
using UnityEngine;

public class VoteSection : DemoMenuSection
{
    public Activity Activity;
    private Dictionary<string, bool> selectedOptionIds = new Dictionary<string, bool>();

    protected override string GetTitle()
    {
        return "Vote";
    }

    protected override void InitGuiElements()
    {
        SetupSelectedOptionIds();
    }

    private void SetupSelectedOptionIds()
    {
        Activity.Poll.Options.ForEach((pollOption) => {
            selectedOptionIds[pollOption.OptionId] = pollOption.IsVotedByMe;
        });
    }

    protected override void DrawSectionBody()
    {
        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButton("Add", () => AddVotes(), style: GSStyles.ShortButton);
        DemoGuiUtils.DrawButton("Set", () => SetVotes(), style: GSStyles.ShortButton);
        DemoGuiUtils.DrawButton("Remove", () => RemoveVotes(), style: GSStyles.ShortButton);
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        Activity.Poll.Options.ForEach((option) => {
            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label("Option Id: ", GSStyles.NormalLabelText);
                GUILayout.Label(option.OptionId, GSStyles.NormalLabelText);
            });
            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label("Text: ", GSStyles.NormalLabelText);
                GUILayout.Label(option.Text, GSStyles.NormalLabelText);
            });
            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label("Image Url: ", GSStyles.NormalLabelText);
                var imageURL = option.Attachment != null ? option.Attachment.ImageUrl : "";
                GUILayout.Label(imageURL, GSStyles.NormalLabelText, GUILayout.MaxWidth(300));
            });
            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label("Video Url: ", GSStyles.NormalLabelText);
                var videoURL = option.Attachment != null ? option.Attachment.VideoUrl : "";
                GUILayout.Label(videoURL, GSStyles.NormalLabelText, GUILayout.MaxWidth(300));
            });
            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label("Vote count: " + option.VoteCount, GSStyles.NormalLabelText);
            });
            DemoGuiUtils.DrawRow(() =>
            {
                bool newValue = GUILayout.Toggle(selectedOptionIds[option.OptionId], "", GSStyles.Toggle);
                if (newValue != selectedOptionIds[option.OptionId])
                {
                    selectedOptionIds[option.OptionId] = newValue;
                }
                var oldValues = new Dictionary<string, bool>(selectedOptionIds);
                if (newValue && !Activity.Poll.AllowMultipleVotes)
                {
                    foreach (KeyValuePair<string, bool> kvp in oldValues)
                    {
                        if (!kvp.Key.Equals(option.OptionId))
                        {
                            selectedOptionIds[kvp.Key] = false;
                        }
                    }
                }
            });
        });
    }

    private void AddVotes()
    {
        var optionIds = CollectSelectedOptionIds();
        Communities.AddVotes(optionIds, Activity.Id, () => {
            _console.LogD("Votes added");
            UpdateActivity();
        }, (error) => {
            _console.LogD("Failed to add votes, error: " + error);
        });
    }

    private void SetVotes()
    {
        Communities.SetVotes(CollectSelectedOptionIds(), Activity.Id, () => {
            _console.LogD("Votes set");
            UpdateActivity();
        }, (error) => {
            _console.LogD("Failed to set votes, error: " + error);
        });
    }

    private void RemoveVotes()
    {
        Communities.RemoveVotes(CollectSelectedOptionIds(), Activity.Id, () => {
            _console.LogD("Votes removed");
            UpdateActivity();
        }, (error) => {
            _console.LogD("Failed to remove votes, error: " + error);
        });
    }

    private void UpdateActivity()
    {
        Communities.GetActivity(Activity.Id, (response) => {
            Activity = response;
            SetupSelectedOptionIds();
        }, (error) => {
            _console.LogD("Failed to refresh activity, error: " + error);
        });
    }

    private HashSet<string> CollectSelectedOptionIds()
    {
        var result = new HashSet<string>();
        foreach (KeyValuePair<string, bool> kvp in selectedOptionIds)
        {
            if (kvp.Value == true)
            {
                result.Add(kvp.Key);
            }
        }
        return result;
    }
}
