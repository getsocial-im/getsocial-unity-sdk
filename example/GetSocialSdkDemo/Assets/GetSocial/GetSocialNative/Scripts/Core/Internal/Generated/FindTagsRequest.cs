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
public partial class FindTagsRequest : TBase
{
  private string _sessionId;
  private string _searchTerm;
  private Pagination _pagination;
  private SGEntity _target;

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

  public string SearchTerm
  {
    get
    {
      return _searchTerm;
    }
    set
    {
      __isset.searchTerm = true;
      this._searchTerm = value;
    }
  }

  public Pagination Pagination
  {
    get
    {
      return _pagination;
    }
    set
    {
      __isset.pagination = true;
      this._pagination = value;
    }
  }

  /// <summary>
  /// when not set, all tags belonging to application will be fetched
  /// </summary>
  public SGEntity Target
  {
    get
    {
      return _target;
    }
    set
    {
      __isset.target = true;
      this._target = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool sessionId;
    public bool searchTerm;
    public bool pagination;
    public bool target;
  }

  public FindTagsRequest() {
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
              SearchTerm = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Struct) {
              Pagination = new Pagination();
              Pagination.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Struct) {
              Target = new SGEntity();
              Target.Read(iprot);
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
      TStruct struc = new TStruct("FindTagsRequest");
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
      if (SearchTerm != null && __isset.searchTerm) {
        field.Name = "searchTerm";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(SearchTerm);
        oprot.WriteFieldEnd();
      }
      if (Pagination != null && __isset.pagination) {
        field.Name = "pagination";
        field.Type = TType.Struct;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        Pagination.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (Target != null && __isset.target) {
        field.Name = "target";
        field.Type = TType.Struct;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        Target.Write(oprot);
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
    StringBuilder __sb = new StringBuilder("FindTagsRequest(");
    bool __first = true;
    if (SessionId != null && __isset.sessionId) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("SessionId: ");
      __sb.Append(SessionId);
    }
    if (SearchTerm != null && __isset.searchTerm) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("SearchTerm: ");
      __sb.Append(SearchTerm);
    }
    if (Pagination != null && __isset.pagination) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Pagination: ");
      __sb.Append(Pagination== null ? "<null>" : Pagination.ToString());
    }
    if (Target != null && __isset.target) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Target: ");
      __sb.Append(Target== null ? "<null>" : Target.ToString());
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}
#endif
