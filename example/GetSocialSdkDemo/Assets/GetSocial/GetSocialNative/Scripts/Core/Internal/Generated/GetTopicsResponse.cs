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
public partial class GetTopicsResponse : TBase
{
  private List<SGTopic> _topics;
  private int _totalNumber;
  private string _nextCursor;

  public List<SGTopic> Topics
  {
    get
    {
      return _topics;
    }
    set
    {
      __isset.topics = true;
      this._topics = value;
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
    public bool topics;
    public bool totalNumber;
    public bool nextCursor;
  }

  public GetTopicsResponse() {
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
                Topics = new List<SGTopic>();
                TList _list25 = iprot.ReadListBegin();
                for( int _i26 = 0; _i26 < _list25.Count; ++_i26)
                {
                  SGTopic _elem27;
                  _elem27 = new SGTopic();
                  _elem27.Read(iprot);
                  Topics.Add(_elem27);
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
      TStruct struc = new TStruct("GetTopicsResponse");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Topics != null && __isset.topics) {
        field.Name = "topics";
        field.Type = TType.List;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, Topics.Count));
          foreach (SGTopic _iter28 in Topics)
          {
            _iter28.Write(oprot);
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
    StringBuilder __sb = new StringBuilder("GetTopicsResponse(");
    bool __first = true;
    if (Topics != null && __isset.topics) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Topics: ");
      __sb.Append(Topics);
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
#endif
