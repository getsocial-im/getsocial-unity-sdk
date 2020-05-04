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


#if !SILVERLIGHT
[Serializable]
#endif
public partial class SGFollower : TBase
{
  private THPublicUser _user;
  private long _createdAt;

  public THPublicUser User
  {
    get
    {
      return _user;
    }
    set
    {
      __isset.user = true;
      this._user = value;
    }
  }

  public long CreatedAt
  {
    get
    {
      return _createdAt;
    }
    set
    {
      __isset.createdAt = true;
      this._createdAt = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool user;
    public bool createdAt;
  }

  public SGFollower() {
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
            if (field.Type == TType.Struct) {
              User = new THPublicUser();
              User.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              CreatedAt = iprot.ReadI64();
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
      TStruct struc = new TStruct("SGFollower");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (User != null && __isset.user) {
        field.Name = "user";
        field.Type = TType.Struct;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        User.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (__isset.createdAt) {
        field.Name = "createdAt";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreatedAt);
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
    StringBuilder __sb = new StringBuilder("SGFollower(");
    bool __first = true;
    if (User != null && __isset.user) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("User: ");
      __sb.Append(User== null ? "<null>" : User.ToString());
    }
    if (__isset.createdAt) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("CreatedAt: ");
      __sb.Append(CreatedAt);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}
#endif
