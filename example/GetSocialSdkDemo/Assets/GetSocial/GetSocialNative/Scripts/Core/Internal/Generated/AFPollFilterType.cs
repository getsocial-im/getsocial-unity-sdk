#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
/**
 * Autogenerated by Thrift Compiler ()
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */

namespace GetSocialSdk.Core 
{
  /// <summary>
  /// #sdk7
  /// </summary>
  public enum AFPollFilterType
  {
    all = 0,
    onlyPolls = 1,
    onlyPollsVotedByMe = 2,
    onlyPollsNotVotedByMe = 3,
    onlyWithoutPolls = 4,
  }
}
#endif