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
  public partial class DDGetEntityFollowersResponse : TBase
  {
    private List<THPublicUser> _followers;
    private int _totalNumber;
    private string _nextCursor;

    public List<THPublicUser> Followers
    {
      get
      {
        return _followers;
      }
      set
      {
        __isset.followers = true;
        this._followers = value;
      }
    }

    public int TotalNumber
    {
      get
      {
        return _totalNumber;
      }
      set
      {
        __isset.totalNumber = true;
        this._totalNumber = value;
      }
    }

    public string NextCursor
    {
      get
      {
        return _nextCursor;
      }
      set
      {
        __isset.nextCursor = true;
        this._nextCursor = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool followers;
      public bool totalNumber;
      public bool nextCursor;
    }

    public DDGetEntityFollowersResponse() {
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
              if (field.Type == TType.List) {
                {
                  Followers = new List<THPublicUser>();
                  TList _list128 = iprot.ReadListBegin();
                  for( int _i129 = 0; _i129 < _list128.Count; ++_i129)
                  {
                    THPublicUser _elem130;
                    _elem130 = new THPublicUser();
                    _elem130.Read(iprot);
                    Followers.Add(_elem130);
                  }
                  iprot.ReadListEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.I32) {
                TotalNumber = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                NextCursor = iprot.ReadString();
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
        TStruct struc = new TStruct("DDGetEntityFollowersResponse");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Followers != null && __isset.followers) {
          field.Name = "followers";
          field.Type = TType.List;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.Struct, Followers.Count));
            foreach (THPublicUser _iter131 in Followers)
            {
              _iter131.Write(oprot);
            }
            oprot.WriteListEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (__isset.totalNumber) {
          field.Name = "totalNumber";
          field.Type = TType.I32;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(TotalNumber);
          oprot.WriteFieldEnd();
        }
        if (NextCursor != null && __isset.nextCursor) {
          field.Name = "nextCursor";
          field.Type = TType.String;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(NextCursor);
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
      StringBuilder __sb = new StringBuilder("DDGetEntityFollowersResponse(");
      bool __first = true;
      if (Followers != null && __isset.followers) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Followers: ");
        __sb.Append(Followers.ToDebugString());
      }
      if (__isset.totalNumber) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("TotalNumber: ");
        __sb.Append(TotalNumber);
      }
      if (NextCursor != null && __isset.nextCursor) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("NextCursor: ");
        __sb.Append(NextCursor);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
