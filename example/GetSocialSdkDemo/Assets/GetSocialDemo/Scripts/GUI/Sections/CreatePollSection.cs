using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Assets.GetSocialDemo.Scripts.Utils;
using GetSocialSdk.Core;
using NUnit.Framework;
using UnityEngine;

public class CreatePollSection : DemoMenuSection
{
    public PostActivityTarget Target;

    private string _text;
    private string _endDate;
    private bool _allowMultipleVotes;
    private List<CreatePollOption> _options = new List<CreatePollOption>();

    protected override string GetTitle()
    {
        return "Create Poll";
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Text: ", GSStyles.NormalLabelText);
            _text = GUILayout.TextField(_text, GSStyles.TextField);
        });
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("End date('dd/MM/yyyy HH:mm:ss' or 'dd/MM/yyy'): ", GSStyles.NormalLabelText);
            _endDate = GUILayout.TextField(_endDate, GSStyles.TextField);
        });

        DemoGuiUtils.DrawRow(() =>
        {
            _allowMultipleVotes = GUILayout.Toggle(_allowMultipleVotes, "", GSStyles.Toggle);
            GUILayout.Label("Allow Multiple Votes", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        });

        if (GUILayout.Button("Add Option", GSStyles.Button))
        {
            _options.Add(new CreatePollOption());
        }
        var copiedOptions = new List<CreatePollOption>(_options);

        copiedOptions.ForEach((option) => {
            option.DrawOption(() => {
                _options.Remove(option);
            });
        });

        if (GUILayout.Button("Create", GSStyles.Button))
        {
            CreatePoll();
        }

    }

    private void CreatePoll()
    {
        var activityContent = new ActivityContent();
        activityContent.Text = _text;
        
        var pollContent = new PollContent();
        pollContent.AllowMultipleVotes = _allowMultipleVotes;
        pollContent.EndDate = ParseDate(_endDate);
        _options.ForEach((optionContent) => {
            var option = optionContent.CollectData();
            pollContent.AddPollOption(option);
        });

        activityContent.Poll = pollContent;
        Communities.PostActivity(activityContent, Target, (result) => {
            _console.LogD("Poll created");
            _text = null;
            _allowMultipleVotes = false;
            _endDate = null;
            _options.Clear();
        }, (error) => {
            _console.LogD("Failed to create poll, error: " + error);
        });
    }

    private static DateTime? ParseDate(string date)
    {
        if (date == null || date.Length == 0)
        {
            return null;
        }
        DateTime dateTime;
        CultureInfo provider = CultureInfo.InvariantCulture;

        if (DateTime.TryParseExact(date, "dd/MM/yyyy HH:mm:ss", provider, new DateTimeStyles(), out dateTime)
            || DateTime.TryParseExact(date, "dd/MM/yyyy", provider, new DateTimeStyles(), out dateTime))
        {
            return dateTime;
        }
        else
        {
            return null;
        }
    }

}
