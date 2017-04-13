# GetSocial Unity SDK

## Version History
### v6.0.0

We have released version 6.0 of our SDK, codename Sangria.  This is a major update that brings a lot of improvements and new features. 

**New:**

+ Data API. With GetSocial's Data API developers can now access the data layer of all our features breaking dependency on our UI and giving them more flexibility.  
+ Dynamic Activity feeds. Developers can now create activity feeds on the fly. They can have a feed per clan, per fraction, per geo, or even level specific.
+ Sticky activities are now Announcements. Developers can now post multiple announcements (announcements are always pinned on top) expiring at different time periods. All their announcements are accessible from one convenient location and can be rescheduled even after they expire. 
+ Auto-initialization. Integration just got easier, SDK will automatically initialize with your app, no need to take care of that.

Learn more about new features in brand new GetSocial Documentation: http://docs.getsocial.im

**Updated:**

+ Simplified API. We, at GetSocial, strive to provide the best integration experience. To achive this we had to rework our public API from scratch, it become easier to use and understand. It won't be possible without breaking changes, GetSocial SKD version 6 is not backward compatible with SDK version 5.

**Removed:**

+ GetSocial Chat is no longer a part of GetSocial services. All customers using Chat via SDK v5 and v4 can continue usage as usual. 

---

### v5.3.0

+ UPDATED underlying Android SDK to [v5.3.2](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v5.3.2)
+ UPDATED underlying iOS SDK to [5.3.1](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.3.1)
+ REMOVED warninigs about showing view from the logs

---

### v5.2.4

+ UPDATED underlying Android SDK to [v5.2.1](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v5.2.1)
+ UPDATED underlying iOS SDK to [5.2.3](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.2.3)

---

### v5.2.3

+ UPDATED underlying iOS SDK to [5.2.2](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.2.2)

---

### v5.2.2

+ FIXED Rare bug the caused occasional crashes on Android

---

### v5.2.1

+ UPDATED underlying iOS SDK to [5.2.1](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.2.1) which improves network communications

---

### v5.2.0

+ ADDED Social Graph API
+ UPDATED underlying iOS SDK to [5.2.0](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.2.0)
+ UPDATED underlying Android SDK to [5.2.0](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v5.2.0)

---

### v5.1.8

+ UPDATED underlying iOS SDK to [5.1.9](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.1.9) which fixes UI landscape issues on iOS 7.

---

### v5.1.7

+ UPDATED underlying iOS SDK to [5.1.8](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.1.8) which fixes UI issues on iOS 8.+

---

### v5.1.6

+ UPDATED underlying iOS SDK to [5.1.7](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.1.7)

---

### v5.1.5

+ FIXED issue with registering invite plugins which are disabled on GetSocial Dashboard.

---

### v5.1.4

+ UPDATED underlying iOS SDK to [5.1.6](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.1.6) which provides a workaround for Facebook invite plugin app requests window displaying under GetSocial view.

---

### v5.1.3

+ UPDATED underlying iOS SDK to [5.1.5](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.1.5)

---

### v5.1.2

+ UPDATED underlying Android SDK to [5.1.4](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v5.1.4)

---

### v5.1.1

+ UPDATED underlying Android SDK to [5.1.2](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v5.1.2)
+ UPDATED underlying iOS SDK to [5.1.4](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.1.4)

---

### v5.1.0

+ UPDATED underlying Android SDK to [5.1.0](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v5.1.0)
+ UPDATED underlying iOS SDK to [5.1.0](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.1.0)
+ ADDED `FacebookMessengerInvitePlugin.cs` for smart invites with usage example. (Android only). Due to Facebook Messenger limitations only link can be sent via intent.
+ FIXED Settings not working if not entering the settings menu of the test app
+ ADDED Test app - added logs when posting activity failed

---

### v5.0.9

+ FIXED Bug on Android of `SetImage` not working properly in `SmartInviteViewBuilder`

---

### v5.0.8

+ UPDATED underlying iOS SDK to [5.0.8](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.0.8)
+ UPDATED Now registering for push notifications on iOS is **ENABLED** by default. Please go to `GetSocial -> Edit Settings` to iOS specific settings if you need to change this behavior.

---

### v5.0.7

+ UPDATED underlying Android SDK to [5.0.9](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v5.0.9)
+ UPDATED underlying iOS SDK to [5.0.7](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.0.7)

---

### v5.0.6

+ UPDATED underlying Android SDK to [5.0.8](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v5.0.8)
+ UPDATED underlying iOS SDK to [5.0.5](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.0.5)

---

### v5.0.5

+ IMPROVED Bug fixes and improvements
+ UPDATED underlying Android SDK to [5.0.7](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v5.0.7)
+ UPDATED underlying iOS SDK to [5.0.4](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.0.4)

---

### v5.0.4

+ FIXED `SetInsets` method of UI Configuration not working properly on iOS

---

### v5.0.3

+ ADEDD `HeaderPaddingTop` and `HeaderPaddingBottom` Ui configuration properties
+ FIXED `SetBasePath` and `SetImagePath` of UI Configuration not working properly on iOS
+ FIXED Problem with font scaling when setting UI Configuration via transaction

---

### v5.0.2

+ ADDED Postprocessor to configure associated domains in XCode project (only for demo app).

---

### v5.0.1

+ UPDATED underlying Android SDK to [5.0.4](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v5.0.4)
+ UPDATED underlying iOS SDK to [5.0.2](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.0.2)

---

### v5.0.0

+ UPDATED underlying Android SDK to [5.0.2](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v5.0.2)
+ UPDATED underlying iOS SDK to [5.0.1](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v5.0.1)
+ ADDED Google Play Games login to the test app
+ ADDED Unity Crashlytics to the test app
+ ADDED All methods regarding user management are now invoked via `GetSocial.Instance.CurrentUser`
+ ADDED Methods to set avatar and display name for the user
+ ADDED Method to reset current user
+ ADDED CoreProperty.HEADER_PADDING_TOP and CoreProperty.HEADER_PADDING_BOTTOM
+ DEPRECATED CoreProperty.TITLE_MARGIN_TOP (use CoreProperty.HEADER_PADDING_TOP instead)
+ ADDED `CoreProperty.ACTIVITY_COMMENT_BG_COLOR` property to configure comments background color
+ IMPROVED Android manifest editor
+ IMPROVED GetSocial Android dependencies libs (`google-gson`, `rxandroid`, `rxjava`) are now shipped separately
+ IMPROVED Renamed `SetOnLayerChangeListener` to `OnWindowStateChangeListener`
+ FIXED compatibility problems with Unity 5

---

### v4.0.4

+ UPDATED underlying Android SDK to [4.0.4](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v4.0.4)
+ UPDATED underlying iOS SDK to [4.0.3](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v4.0.3)
+ FIXED referral data not received when clicking on the smart invite while the app is in background by introducing`GetSocialDeepLinkingActivity`. Check the docs [how to update](http://docs.getsocial.im/#smart-invites) your `AndroidManifest.xml`.
+ FIXED errors when creating `AndroidManifest.xml` file from manifest editor in some Unity 5 versions
+ FIXED `CreateChatViewForUserIdOnProvider` not working properly on iOS

---

### v4.0.3

+ UPDATED underlying Android SDK to [4.0.3](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v4.0.3)
+ UPDATED underlying iOS SDK to [4.0.2](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v4.0.2)
+ ADDED referralDataUrl parameter to Facebook smart invite callback
+ IMPROVED GetSocial `AndroidManifest.xml` editor. Added checks for smart invites configuration (image provider authority)
+ FIXED window titles sometimes missing
+ FIXED push notifications added to notification center when GetSocial views are open

---

### v4.0.2

+ UPDATED underlying Android SDK to [4.0.2](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v4.0.2)
+ ADDED `AndroidManifest.xml` editor to GetSocial Settings inspector.
+ FIXED Test app calling some native methods each frame.

---

### v4.0.1

+ UPDATED underlying Android SDK to [4.0.1](https://github.com/getsocial-im/getsocial-android-sdk/releases/tag/v4.0.1)
+ UPDATED underlying iOS SDK to [4.0.1](https://github.com/getsocial-im/getsocial-ios-sdk/releases/tag/v4.0.1)
+ FIXED loading UI configuration from JSON file from URL not working properly
+ ADDED `AnimationStyle.None` to let developers disable window animations

---

### v4.0.0

Release v4.0.0 brings a lot of new features, improvements and few breaking changes. The most notable are:

+ Integration guide and reference migrated to [docs.getsocial.im](http://docs.getsocial.im).
+ Now GetSocial is spitted into `Core` (Activity Feed, Notifications, Smart Invites) and `Chat` modules. [Learn more about migration from version 3.x.](http://docs.getsocial.im/#upgrade-guide)
+ New UI configuration system. Now all UI properties can be customized via JSON configuration file. [Learn more...](http://docs.getsocial.im/ui-customization/#developers-guide)
+ Big part of `GetSocial` methods were refactored to more meaningful names (e.g. `authenticateGame(...)` => `init(...)`, `verifyUserIdentity(...)` => `login(...)`, etc.
+ Added support for linking multiple user accounts. [Learn more...](http://docs.getsocial.im/#adding-identity-info)
+ Replaced generic `open(...)` view method with sophisticated builders API. To obtain view builder call `createXxxView()`.
+ Added friends list view. [Learn more...](http://docs.getsocial.im/#friends-list)
+ Now users can receive notifications about likes and comments from application account.

---

### v3.5.0

+ ADDED ReferralData on SmartInvites
+ ADDED onReferralDataReceivedHandler
+ ADDED OnInviteButtonClickHandler
+ ADDED support for deeplinking

---

### v3.4.1

+ FIXED crash on Android when "Split Application Binary" is enabled

---

### v3.4.0

+ ADDED support for activities with image, button, action
+ ADDED `OnGameAvatarClick` listener
+ ADDED `OnActivityActionClick` listener
+ ADDED Leaderboards support
+ ADDED ability to disable chat from GetSocial web dashboard
+ FIXED Xcode project post+processing when project path had spaces
+ FIXED exception on post activity with tags

---

### v3.3.0

+ ADDED Global Chat Room support
+ ADDED `UnreadNotificationsCount` and `UnreadConversationsCount` properties
+ FIXED issue with callbacks executed on background thread

---

### v3.2.0

+ Initial GetSocial Unity SDK release.
