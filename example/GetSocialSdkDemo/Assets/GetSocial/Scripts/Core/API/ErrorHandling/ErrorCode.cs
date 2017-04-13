namespace GetSocialSdk.Core
{
    /// <summary>
    /// Error codes number is specified according to layer where error can happen.
    /// </summary>
    public static class ErrorCodes
    {
        public const int Unknown = -1;

        #region generic errors

        public const int CompositeException = 100;

        #endregion

        #region GetSocial public API errors

        public const int ActionDenied = 201;
        public const int SdkNotInitialized = 202;
        public const int SdkInitializationFailed = 203;
        public const int InvalidArgument = 204;

        #endregion

        #region GetSocial private API errors

        public const int InviteCancelled = 100;

        #endregion

        #region business logic layer errors (in UseCase's or Func'tions)

        /// <summary>
        /// Error code indicates that API returned unexpected
        // response that was correctly parsed by business logic layer does not know how to handle it.
        /// </summary>
        public const int UnexpectedApiResponse = 401;
        public const int NotFound = 404;

        #endregion

        #region parser layer errors

        public const int JsonParsingException = 501;

        #endregion

        #region wamp layer errors

        public const int GenericWampClientError = 600;
        public const int WampClientInitializationError = 601;

        #endregion

        #region communication layer errors

        public const int ConnectionTimeout = 701;
        public const int NoInternet = 702;
        public const int TransportClosed = 703;

        #endregion
    }
}