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
  /// #sdk6 #sdk7
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class THPushTarget : TBase
  {
    private string _token;
    private string _language;
    private THDeviceOs _platformId;
    private bool _iosSandbox;

    public string Token
    {
      get
      {
        return _token;
      }
      set
      {
        __isset.token = true;
        this._token = value;
      }
    }

    public string Language
    {
      get
      {
        return _language;
      }
      set
      {
        __isset.language = true;
        this._language = value;
      }
    }

    /// <summary>
    /// 
    /// <seealso cref=".THDeviceOs"/>
    /// </summary>
    public THDeviceOs PlatformId
    {
      get
      {
        return _platformId;
      }
      set
      {
        __isset.platformId = true;
        this._platformId = value;
      }
    }

    public bool IosSandbox
    {
      get
      {
        return _iosSandbox;
      }
      set
      {
        __isset.iosSandbox = true;
        this._iosSandbox = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool token;
      public bool language;
      public bool platformId;
      public bool iosSandbox;
    }

    public THPushTarget() {
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
                Token = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                Language = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.I32) {
                PlatformId = (THDeviceOs)iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.Bool) {
                IosSandbox = iprot.ReadBool();
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
        TStruct struc = new TStruct("THPushTarget");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Token != null && __isset.token) {
          field.Name = "token";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Token);
          oprot.WriteFieldEnd();
        }
        if (Language != null && __isset.language) {
          field.Name = "language";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Language);
          oprot.WriteFieldEnd();
        }
        if (__isset.platformId) {
          field.Name = "platformId";
          field.Type = TType.I32;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32((int)PlatformId);
          oprot.WriteFieldEnd();
        }
        if (__isset.iosSandbox) {
          field.Name = "iosSandbox";
          field.Type = TType.Bool;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(IosSandbox);
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
      StringBuilder __sb = new StringBuilder("THPushTarget(");
      bool __first = true;
      if (Token != null && __isset.token) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Token: ");
        __sb.Append(Token);
      }
      if (Language != null && __isset.language) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Language: ");
        __sb.Append(Language);
      }
      if (__isset.platformId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("PlatformId: ");
        __sb.Append(PlatformId);
      }
      if (__isset.iosSandbox) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("IosSandbox: ");
        __sb.Append(IosSandbox);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
