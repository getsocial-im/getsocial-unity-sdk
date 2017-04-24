using Facebook.Unity;
using GetSocialSdk.Core;
using UnityEngine;

#if USE_GETSOCIAL_UI
using GetSocialSdk.Ui;

public class ActivityFeedUiSection : DemoMenuSection
{
    string _feed = "default";

    protected override string GetTitle()
    {
        return "Activity Feed UI";
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawButton("Global Feed", OpenGlobalFeed, true, GSStyles.Button);
        DemoGuiUtils.Space();

        // feed by id
        _feed = GUILayout.TextField(_feed, GSStyles.TextField);
        DemoGuiUtils.DrawButton("Feed By Id", OpenFeedWithId, true, GSStyles.Button);
    }

    void OpenFeedWithId()
    {
        GetSocialUi.СreateActivityFeedView(_feed)
            .SetWindowTitle(_feed + " Title")
            .SetButtonActionListener(OnActivityActionClicked)
            .Show();
    }

    void OpenGlobalFeed()
    {
        GetSocialUi.СreateGlobalActivityFeedView()
            .SetWindowTitle("Unity Global")
            .SetViewStateCallbacks(() => _console.LogD("Global feed opened"), () => _console.LogD("Global feed closed"))
            .SetButtonActionListener(OnActivityActionClicked)
            .Show();
    }

    void OnActivityActionClicked(string actionId, ActivityPost post)
    {
        _console.LogD(string.Format("[{0}] button clicked on post: {1}", actionId, post));
    }
}

#endif