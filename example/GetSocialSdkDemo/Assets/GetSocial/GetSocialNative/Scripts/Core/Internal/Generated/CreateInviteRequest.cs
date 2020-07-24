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
  /// #todo_sdk7
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class CreateInviteRequest : TBase
  {
    private string _sessionId;
    private THInviteContentV2 _inviteContent;
    private string _providerId;
    private THInviteType _type;

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

    public THInviteContentV2 InviteContent
    {
      get
      {
        return _inviteContent;
      }
      set
      {
        __isset.inviteContent = true;
        this._inviteContent = value;
      }
    }

    public string ProviderId
    {
      get
      {
        return _providerId;
      }
      set
      {
        __isset.providerId = true;
        this._providerId = value;
      }
    }

    /// <summary>
    /// e.g facebook SDK won't set this if only creating content but not sending invite
    /// 
    /// <seealso cref="THInviteType"/>
    /// </summary>
    public THInviteType Type
    {
      get
      {
        return _type;
      }
      set
      {
        __isset.type = true;
        this._type = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sessionId;
      public bool inviteContent;
      public bool providerId;
      public bool type;
    }

    public CreateInviteRequest() {
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
              if (field.Type == TType.Struct) {
                InviteContent = new THInviteContentV2();
                InviteContent.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                ProviderId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.I32) {
                Type = (THInviteType)iprot.ReadI32();
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
        TStruct struc = new TStruct("CreateInviteRequest");
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
        if (InviteContent != null && __isset.inviteContent) {
          field.Name = "inviteContent";
          field.Type = TType.Struct;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          InviteContent.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (ProviderId != null && __isset.providerId) {
          field.Name = "providerId";
          field.Type = TType.String;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(ProviderId);
          oprot.WriteFieldEnd();
        }
        if (__isset.type) {
          field.Name = "type";
          field.Type = TType.I32;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32((int)Type);
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
      StringBuilder __sb = new StringBuilder("CreateInviteRequest(");
      bool __first = true;
      if (SessionId != null && __isset.sessionId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("SessionId: ");
        __sb.Append(SessionId);
      }
      if (InviteContent != null && __isset.inviteContent) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("InviteContent: ");
        __sb.Append(InviteContent== null ? "<null>" : InviteContent.ToString());
      }
      if (ProviderId != null && __isset.providerId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ProviderId: ");
        __sb.Append(ProviderId);
      }
      if (__isset.type) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Type: ");
        __sb.Append(Type);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
