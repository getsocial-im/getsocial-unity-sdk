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
using Thrift.Protocol;
using Thrift.Transport;

namespace GetSocialSdk.Core 
{

  /// <summary>
  /// #sdk7
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class AreGroupMembersRequest : TBase
  {
    private string _sessionId;
    private string _groupId;
    private List<string> _userIds;

    public string SessionId
    {
      get
      {
        return _sessionId;
      }
      set
      {
        __isset.sessionId = true;
        this._sessionId = value;
      }
    }

    public string GroupId
    {
      get
      {
        return _groupId;
      }
      set
      {
        __isset.groupId = true;
        this._groupId = value;
      }
    }

    public List<string> UserIds
    {
      get
      {
        return _userIds;
      }
      set
      {
        __isset.userIds = true;
        this._userIds = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sessionId;
      public bool groupId;
      public bool userIds;
    }

    public AreGroupMembersRequest() {
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.String) {
                SessionId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                GroupId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.List) {
                {
                  UserIds = new List<string>();
                  TList _list91 = iprot.ReadListBegin();
                  for( int _i92 = 0; _i92 < _list91.Count; ++_i92)
                  {
                    string _elem93;
                    _elem93 = iprot.ReadString();
                    UserIds.Add(_elem93);
                  }
                  iprot.ReadListEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("AreGroupMembersRequest");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (SessionId != null && __isset.sessionId) {
          field.Name = "sessionId";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(SessionId);
          oprot.WriteFieldEnd();
        }
        if (GroupId != null && __isset.groupId) {
          field.Name = "groupId";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(GroupId);
          oprot.WriteFieldEnd();
        }
        if (UserIds != null && __isset.userIds) {
          field.Name = "userIds";
          field.Type = TType.List;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.String, UserIds.Count));
            foreach (string _iter94 in UserIds)
            {
              oprot.WriteString(_iter94);
            }
            oprot.WriteListEnd();
          }
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("AreGroupMembersRequest(");
      bool __first = true;
      if (SessionId != null && __isset.sessionId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("SessionId: ");
        __sb.Append(SessionId);
      }
      if (GroupId != null && __isset.groupId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("GroupId: ");
        __sb.Append(GroupId);
      }
      if (UserIds != null && __isset.userIds) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("UserIds: ");
        __sb.Append(UserIds.ToDebugString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
