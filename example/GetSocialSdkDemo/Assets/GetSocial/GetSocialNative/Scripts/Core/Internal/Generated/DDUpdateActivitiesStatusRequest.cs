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

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class DDUpdateActivitiesStatusRequest : TBase
  {
    private string _sessionId;
    private string _appId;
    private THashSet<string> _ids;
    private string _status;
    private AFPollContent _poll;
    private bool _allowMultiReactions;

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

    public string AppId
    {
      get
      {
        return _appId;
      }
      set
      {
        __isset.appId = true;
        this._appId = value;
      }
    }

    public THashSet<string> Ids
    {
      get
      {
        return _ids;
      }
      set
      {
        __isset.ids = true;
        this._ids = value;
      }
    }

    public string Status
    {
      get
      {
        return _status;
      }
      set
      {
        __isset.status = true;
        this._status = value;
      }
    }

    /// <summary>
    /// approved, rejected, deleted
    /// </summary>
    public AFPollContent Poll
    {
      get
      {
        return _poll;
      }
      set
      {
        __isset.poll = true;
        this._poll = value;
      }
    }

    public bool AllowMultiReactions
    {
      get
      {
        return _allowMultiReactions;
      }
      set
      {
        __isset.allowMultiReactions = true;
        this._allowMultiReactions = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sessionId;
      public bool appId;
      public bool ids;
      public bool status;
      public bool poll;
      public bool allowMultiReactions;
    }

    public DDUpdateActivitiesStatusRequest() {
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
                AppId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.Set) {
                {
                  Ids = new THashSet<string>();
                  TSet _set213 = iprot.ReadSetBegin();
                  for( int _i214 = 0; _i214 < _set213.Count; ++_i214)
                  {
                    string _elem215;
                    _elem215 = iprot.ReadString();
                    Ids.Add(_elem215);
                  }
                  iprot.ReadSetEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                Status = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.Struct) {
                Poll = new AFPollContent();
                Poll.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.Bool) {
                AllowMultiReactions = iprot.ReadBool();
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
        TStruct struc = new TStruct("DDUpdateActivitiesStatusRequest");
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
        if (AppId != null && __isset.appId) {
          field.Name = "appId";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(AppId);
          oprot.WriteFieldEnd();
        }
        if (Ids != null && __isset.ids) {
          field.Name = "ids";
          field.Type = TType.Set;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteSetBegin(new TSet(TType.String, Ids.Count));
            foreach (string _iter216 in Ids)
            {
              oprot.WriteString(_iter216);
            }
            oprot.WriteSetEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (Status != null && __isset.status) {
          field.Name = "status";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Status);
          oprot.WriteFieldEnd();
        }
        if (Poll != null && __isset.poll) {
          field.Name = "poll";
          field.Type = TType.Struct;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          Poll.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (__isset.allowMultiReactions) {
          field.Name = "allowMultiReactions";
          field.Type = TType.Bool;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(AllowMultiReactions);
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
      StringBuilder __sb = new StringBuilder("DDUpdateActivitiesStatusRequest(");
      bool __first = true;
      if (SessionId != null && __isset.sessionId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("SessionId: ");
        __sb.Append(SessionId);
      }
      if (AppId != null && __isset.appId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("AppId: ");
        __sb.Append(AppId);
      }
      if (Ids != null && __isset.ids) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Ids: ");
        __sb.Append(Ids);
      }
      if (Status != null && __isset.status) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Status: ");
        __sb.Append(Status);
      }
      if (Poll != null && __isset.poll) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Poll: ");
        __sb.Append(Poll== null ? "<null>" : Poll.ToString());
      }
      if (__isset.allowMultiReactions) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("AllowMultiReactions: ");
        __sb.Append(AllowMultiReactions);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
