/**
 *     Copyright 2015-2019 GetSocial B.V.
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

using GetSocialSdk.Core;
using GetSocialSdk.Ui;

public class NotificationUiSection : DemoMenuSection 
{
 
    #region implemented abstract members of DemoMenuSection

    protected override string GetTitle()
    {
        return "Notification Center";
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawButton("Open Notification Center", ShowNotificationCenterView, style: GSStyles.Button);
    }

    #endregion

    void ShowNotificationCenterView()
    {
        GetSocialUi.CreateNotificationCenterView()
            .SetNotificationClickListener((notification) =>
            {
                _console.LogD("Notification click listener invoked: " + notification.Id);
                return demoController.HandleAction(notification.NotificationAction);
            })
            .SetActionButtonClickListener((notification, actionButton) =>
            {
                _console.LogD("Action button listener invoked: " + actionButton.Id + " - " + notification.Id);
                return false;
            })
            .SetViewStateCallbacks(() => _console.LogD("Notifications view opened"), () => _console.LogD("Notifications view closed"))
            .Show();
    }
 
}
