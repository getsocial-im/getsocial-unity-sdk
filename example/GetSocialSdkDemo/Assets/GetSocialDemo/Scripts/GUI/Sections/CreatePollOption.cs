using System;
using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.Core;
using UnityEngine;

public class CreatePollOption
{
    private string _optionId;
    private string _text;
    private string _imageUrl;
    private string _videoUrl;
    
    public void DrawOption(Action onRemove)
    {
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Option Id: ", GSStyles.NormalLabelText);
            _optionId = GUILayout.TextField(_optionId, GSStyles.TextField);
        });
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Text: ", GSStyles.NormalLabelText);
            _text = GUILayout.TextField(_text, GSStyles.TextField);
        });
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Image Url: ", GSStyles.NormalLabelText);
            _imageUrl = GUILayout.TextField(_imageUrl, GSStyles.TextField);
        });
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Video Url: ", GSStyles.NormalLabelText);
            _videoUrl = GUILayout.TextField(_videoUrl, GSStyles.TextField);
        });

        DemoGuiUtils.DrawRow(() =>
        {
            if (GUILayout.Button("Remove", GSStyles.Button)) {
                onRemove();
            }
        });

    }

    public PollOptionContent CollectData()
    {
        var content = new PollOptionContent();
        content.OptionId = _optionId;
        content.Text = _text;
        if (_imageUrl != null && _imageUrl.Length > 0)
        {
            content.Attachment = MediaAttachment.WithImageUrl(_imageUrl);
        }
        else if (_videoUrl != null && _videoUrl.Length > 0)
        {
            content.Attachment = MediaAttachment.WithVideoUrl(_videoUrl);
        }

        return content;
    }
}
