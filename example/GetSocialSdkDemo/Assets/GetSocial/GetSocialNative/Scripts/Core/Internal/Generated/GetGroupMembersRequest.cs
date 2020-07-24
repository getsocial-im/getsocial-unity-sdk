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
  public partial class GetGroupMembersRequest : TBase
  {
    private string _sessionId;
    private string _groupId;
    private Pagination _pagination;
    private SGMembershipInfo _info;

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

    public Pagination Pagination
    {
      get
      {
        return _pagination;
      }
      set
      {
        __isset.pagination = true;
        this._pagination = value;
      }
    }

    public SGMembershipInfo Info
    {
      get
      {
        return _info;
      }
      set
      {
        __isset.info = true;
        this._info = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sessionId;
      public bool groupId;
      public bool pagination;
      public bool info;
    }

    public GetGroupMembersRequest() {
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
              if (field.Type == TType.Struct) {
                Pagination = new Pagination();
                Pagination.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.Struct) {
                Info = new SGMembershipInfo();
                Info.Read(iprot);
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
        TStruct struc = new TStruct("GetGroupMembersRequest");
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
        if (Pagination != null && __isset.pagination) {
          field.Name = "pagination";
          field.Type = TType.Struct;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          Pagination.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (Info != null && __isset.info) {
          field.Name = "info";
          field.Type = TType.Struct;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          Info.Write(oprot);
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
      StringBuilder __sb = new StringBuilder("GetGroupMembersRequest(");
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
      if (Pagination != null && __isset.pagination) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Pagination: ");
        __sb.Append(Pagination== null ? "<null>" : Pagination.ToString());
      }
      if (Info != null && __isset.info) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Info: ");
        __sb.Append(Info== null ? "<null>" : Info.ToString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
