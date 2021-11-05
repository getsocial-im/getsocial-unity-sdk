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
  /// SGMemberStatus
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class DDGetGroupMembersResponse : TBase
  {
    private List<SGGroupMember> _members;
    private int _totalNumber;
    private string _nextCursor;

    public List<SGGroupMember> Members
    {
      get
      {
        return _members;
      }
      set
      {
        __isset.members = true;
        this._members = value;
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
      public bool members;
      public bool totalNumber;
      public bool nextCursor;
    }

    public DDGetGroupMembersResponse() {
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
                  Members = new List<SGGroupMember>();
                  TList _list165 = iprot.ReadListBegin();
                  for( int _i166 = 0; _i166 < _list165.Count; ++_i166)
                  {
                    SGGroupMember _elem167;
                    _elem167 = new SGGroupMember();
                    _elem167.Read(iprot);
                    Members.Add(_elem167);
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
        TStruct struc = new TStruct("DDGetGroupMembersResponse");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Members != null && __isset.members) {
          field.Name = "members";
          field.Type = TType.List;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.Struct, Members.Count));
            foreach (SGGroupMember _iter168 in Members)
            {
              _iter168.Write(oprot);
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
      StringBuilder __sb = new StringBuilder("DDGetGroupMembersResponse(");
      bool __first = true;
      if (Members != null && __isset.members) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Members: ");
        __sb.Append(Members.ToDebugString());
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
