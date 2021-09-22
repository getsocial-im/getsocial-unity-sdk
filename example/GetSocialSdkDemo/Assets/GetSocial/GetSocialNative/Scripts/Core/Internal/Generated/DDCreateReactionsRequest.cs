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
  public partial class DDCreateReactionsRequest : TBase
  {
    private string _sessionId;
    private string _appId;
    private THashSet<SGEntity> _ids;
    private string _reaction;
    private bool _keepExisting;

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

    public THashSet<SGEntity> Ids
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

    public string Reaction
    {
      get
      {
        return _reaction;
      }
      set
      {
        __isset.reaction = true;
        this._reaction = value;
      }
    }

    public bool KeepExisting
    {
      get
      {
        return _keepExisting;
      }
      set
      {
        __isset.keepExisting = true;
        this._keepExisting = value;
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
      public bool reaction;
      public bool keepExisting;
    }

    public DDCreateReactionsRequest() {
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
                  Ids = new THashSet<SGEntity>();
                  TSet _set281 = iprot.ReadSetBegin();
                  for( int _i282 = 0; _i282 < _set281.Count; ++_i282)
                  {
                    SGEntity _elem283;
                    _elem283 = new SGEntity();
                    _elem283.Read(iprot);
                    Ids.Add(_elem283);
                  }
                  iprot.ReadSetEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                Reaction = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.Bool) {
                KeepExisting = iprot.ReadBool();
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
        TStruct struc = new TStruct("DDCreateReactionsRequest");
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
            oprot.WriteSetBegin(new TSet(TType.Struct, Ids.Count));
            foreach (SGEntity _iter284 in Ids)
            {
              _iter284.Write(oprot);
            }
            oprot.WriteSetEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (Reaction != null && __isset.reaction) {
          field.Name = "reaction";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Reaction);
          oprot.WriteFieldEnd();
        }
        if (__isset.keepExisting) {
          field.Name = "keepExisting";
          field.Type = TType.Bool;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(KeepExisting);
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
      StringBuilder __sb = new StringBuilder("DDCreateReactionsRequest(");
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
      if (Reaction != null && __isset.reaction) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Reaction: ");
        __sb.Append(Reaction);
      }
      if (__isset.keepExisting) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("KeepExisting: ");
        __sb.Append(KeepExisting);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
