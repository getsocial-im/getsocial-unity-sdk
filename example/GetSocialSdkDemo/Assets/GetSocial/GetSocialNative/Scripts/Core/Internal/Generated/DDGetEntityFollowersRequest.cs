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
  public partial class DDGetEntityFollowersRequest : TBase
  {
    private string _sessionId;
    private string _appId;
    private SGEntity _id;
    private string _searchTerm;
    private Pagination _pagination;

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


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sessionId;
      public bool appId;
      public bool id;
      public bool searchTerm;
      public bool pagination;
    }

    public DDGetEntityFollowersRequest() {
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
              if (field.Type == TType.Struct) {
                Id = new SGEntity();
                Id.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                SearchTerm = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.Struct) {
                Pagination = new Pagination();
                Pagination.Read(iprot);
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
        TStruct struc = new TStruct("DDGetEntityFollowersRequest");
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
        if (Id != null && __isset.id) {
          field.Name = "id";
          field.Type = TType.Struct;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          Id.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (SearchTerm != null && __isset.searchTerm) {
          field.Name = "searchTerm";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(SearchTerm);
          oprot.WriteFieldEnd();
        }
        if (Pagination != null && __isset.pagination) {
          field.Name = "pagination";
          field.Type = TType.Struct;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          Pagination.Write(oprot);
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
      StringBuilder __sb = new StringBuilder("DDGetEntityFollowersRequest(");
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
      if (Id != null && __isset.id) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Id: ");
        __sb.Append(Id== null ? "<null>" : Id.ToString());
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
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif