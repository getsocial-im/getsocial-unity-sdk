#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
/**
 * Autogenerated by Thrift Compiler ()
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;

namespace GetSocialSdk.Core 
{
  public static class irisConstants
  {
    public const string IrisNotificationsTypeComment = "comment";
    public const string IrisNotificationsTypeLikeActivity = "activity_like";
    public const string IrisNotificationsTypeLikeComment = "comment_like";
    /// <summary>
    /// now this numbers are vacant for some other notifications types
    /// </summary>
    public const string IrisNotificationsTypeRelatedComment = "related_comment";
    public const string IrisNotificationsTypeNewFriendship = "friends_add";
    public const string IrisNotificationsTypeInviteAccepted = "invite_accept";
    public const string IrisNotificationsTypeMentionComment = "comment_mention";
    public const string IrisNotificationsTypeMentionActivity = "activity_mention";
    public const string IrisNotificationsTypeReplyComment = "comment_reply";
    public const string IrisNotificationsTypeTargeting = "targeting";
    public const string IrisNotificationsTypeDirect = "direct";
  }
}
#endif
