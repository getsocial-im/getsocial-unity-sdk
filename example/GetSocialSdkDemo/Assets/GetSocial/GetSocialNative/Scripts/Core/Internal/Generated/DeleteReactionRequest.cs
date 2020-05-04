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
public partial class DeleteReactionRequest : TBase
{
  private string _sessionId;
  private SGEntity _id;
  private string _reaction;

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

  public SGEntity Id
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


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool sessionId;
    public bool id;
    public bool reaction;
  }

  public DeleteReactionRequest() {
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
              Id = new SGEntity();
              Id.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              Reaction = iprot.ReadString();
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
      TStruct struc = new TStruct("DeleteReactionRequest");
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
      if (Id != null && __isset.id) {
        field.Name = "id";
        field.Type = TType.Struct;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        Id.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (Reaction != null && __isset.reaction) {
        field.Name = "reaction";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Reaction);
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
    StringBuilder __sb = new StringBuilder("DeleteReactionRequest(");
    bool __first = true;
    if (SessionId != null && __isset.sessionId) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("SessionId: ");
      __sb.Append(SessionId);
    }
    if (Id != null && __isset.id) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Id: ");
      __sb.Append(Id== null ? "<null>" : Id.ToString());
    }
    if (Reaction != null && __isset.reaction) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Reaction: ");
      __sb.Append(Reaction);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}
#endif
