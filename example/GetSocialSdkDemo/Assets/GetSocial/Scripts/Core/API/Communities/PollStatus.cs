namespace GetSocialSdk.Core
{
    public static class PollStatus
    {
        public const int All = 0;
        public const int WithPoll = 1;
        public const int WithPollVotedByMe = 2;
        public const int WithPollNotVotedByMe = 3;
        public const int WithoutPoll = 4;
    }
}