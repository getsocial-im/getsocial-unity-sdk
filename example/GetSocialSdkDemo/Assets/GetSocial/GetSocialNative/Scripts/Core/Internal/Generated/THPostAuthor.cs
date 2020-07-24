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
  /// #sdk6
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class THPostAuthor : TBase
  {
    private string _id;
    private string _displayName;
    private string _avatarUrl;
    private List<THIdentity> _identities;
    private bool _verified;
    private bool _isApp;
    private Dictionary<string, string> _publicProperties;

    public string Id
    {
      get
      {
        return _id;
      }
      set
      {
        __isset.id = true;
        this._id = value;
      }
    }

    public string DisplayName
    {
      get
      {
        return _displayName;
      }
      set
      {
        __isset.displayName = true;
        this._displayName = value;
      }
    }

    public string AvatarUrl
    {
      get
      {
        return _avatarUrl;
      }
      set
      {
        __isset.avatarUrl = true;
        this._avatarUrl = value;
      }
    }

    public List<THIdentity> Identities
    {
      get
      {
        return _identities;
      }
      set
      {
        __isset.identities = true;
        this._identities = value;
      }
    }

    public bool Verified
    {
      get
      {
        return _verified;
      }
      set
      {
        __isset.verified = true;
        this._verified = value;
      }
    }

    public bool IsApp
    {
      get
      {
        return _isApp;
      }
      set
      {
        __isset.isApp = true;
        this._isApp = value;
      }
    }

    public Dictionary<string, string> PublicProperties
    {
      get
      {
        return _publicProperties;
      }
      set
      {
        __isset.publicProperties = true;
        this._publicProperties = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool id;
      public bool displayName;
      public bool avatarUrl;
      public bool identities;
      public bool verified;
      public bool isApp;
      public bool publicProperties;
    }

    public THPostAuthor() {
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
                Id = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                DisplayName = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                AvatarUrl = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.List) {
                {
                  Identities = new List<THIdentity>();
                  TList _list62 = iprot.ReadListBegin();
                  for( int _i63 = 0; _i63 < _list62.Count; ++_i63)
                  {
                    THIdentity _elem64;
                    _elem64 = new THIdentity();
                    _elem64.Read(iprot);
                    Identities.Add(_elem64);
                  }
                  iprot.ReadListEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.Bool) {
                Verified = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.Bool) {
                IsApp = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 7:
              if (field.Type == TType.Map) {
                {
                  PublicProperties = new Dictionary<string, string>();
                  TMap _map65 = iprot.ReadMapBegin();
                  for( int _i66 = 0; _i66 < _map65.Count; ++_i66)
                  {
                    string _key67;
                    string _val68;
                    _key67 = iprot.ReadString();
                    _val68 = iprot.ReadString();
                    PublicProperties[_key67] = _val68;
                  }
                  iprot.ReadMapEnd();
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
        TStruct struc = new TStruct("THPostAuthor");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Id != null && __isset.id) {
          field.Name = "id";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Id);
          oprot.WriteFieldEnd();
        }
        if (DisplayName != null && __isset.displayName) {
          field.Name = "displayName";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(DisplayName);
          oprot.WriteFieldEnd();
        }
        if (AvatarUrl != null && __isset.avatarUrl) {
          field.Name = "avatarUrl";
          field.Type = TType.String;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(AvatarUrl);
          oprot.WriteFieldEnd();
        }
        if (Identities != null && __isset.identities) {
          field.Name = "identities";
          field.Type = TType.List;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.Struct, Identities.Count));
            foreach (THIdentity _iter69 in Identities)
            {
              _iter69.Write(oprot);
            }
            oprot.WriteListEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (__isset.verified) {
          field.Name = "verified";
          field.Type = TType.Bool;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(Verified);
          oprot.WriteFieldEnd();
        }
        if (__isset.isApp) {
          field.Name = "isApp";
          field.Type = TType.Bool;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(IsApp);
          oprot.WriteFieldEnd();
        }
        if (PublicProperties != null && __isset.publicProperties) {
          field.Name = "publicProperties";
          field.Type = TType.Map;
          field.ID = 7;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteMapBegin(new TMap(TType.String, TType.String, PublicProperties.Count));
            foreach (string _iter70 in PublicProperties.Keys)
            {
              oprot.WriteString(_iter70);
              oprot.WriteString(PublicProperties[_iter70]);
            }
            oprot.WriteMapEnd();
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
      StringBuilder __sb = new StringBuilder("THPostAuthor(");
      bool __first = true;
      if (Id != null && __isset.id) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Id: ");
        __sb.Append(Id);
      }
      if (DisplayName != null && __isset.displayName) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("DisplayName: ");
        __sb.Append(DisplayName);
      }
      if (AvatarUrl != null && __isset.avatarUrl) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("AvatarUrl: ");
        __sb.Append(AvatarUrl);
      }
      if (Identities != null && __isset.identities) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Identities: ");
        __sb.Append(Identities.ToDebugString());
      }
      if (__isset.verified) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Verified: ");
        __sb.Append(Verified);
      }
      if (__isset.isApp) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("IsApp: ");
        __sb.Append(IsApp);
      }
      if (PublicProperties != null && __isset.publicProperties) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("PublicProperties: ");
        __sb.Append(PublicProperties.ToDebugString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif