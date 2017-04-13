typedef void(VoidCallbackDelegate)(void *actionPtr);

typedef void(BoolCallbackDelegate)(void *actionPtr, bool result);

typedef void(IntCallbackDelegate)(void *actionPtr, int result);

typedef void(StringCallbackDelegate)(void *actionPtr, const char *data);

typedef void(FailureCallbackDelegate)(void *actionPtr, const char *error);

typedef void(FailureWithDataCallbackDelegate)(void *actionPtr, const char *data, const char *error);

typedef void(GlobalErrorCallbackDelegate)(void *onGlobalErrorPtr, const char *errorJson);

typedef void(FetchReferralDataCallbackDelegate)(void *actionPtr, const char *referralDataJson);

typedef void(OnUserConflictDelegate)(void *onConflictActionPtr, const char *conflictUserJson);

typedef bool(NotificationActionListener)(void *funcPtr, const char *notificationAction);
