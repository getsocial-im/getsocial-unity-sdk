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
  /// #sdk7
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class GetGroupsRequest : TBase
  {
    private string _sessionId;
    private Pagination _pagination;
    private string _searchTerm;
    private string _followedByUserId;
    private string _memberUserId;
    private bool _isTrending;
    private string _orderBy;

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
    /// 3, 4, 5 are mutually exclusive
    /// </summary>
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

    /// <summary>
    /// search in name and description
    /// </summary>
    public string FollowedByUserId
    {
      get
      {
        return _followedByUserId;
      }
      set
      {
        __isset.followedByUserId = true;
        this._followedByUserId = value;
      }
    }

    /// <summary>
    /// all groups followed by some user
    /// </summary>
    public string MemberUserId
    {
      get
      {
        return _memberUserId;
      }
      set
      {
        __isset.memberUserId = true;
        this._memberUserId = value;
      }
    }

    /// <summary>
    /// all groups that some user is a member of, supports both identity id(provider:id) and user id
    /// </summary>
    public bool IsTrending
    {
      get
      {
        return _isTrending;
      }
      set
      {
        __isset.isTrending = true;
        this._isTrending = value;
      }
    }

    /// <summary>
    /// options: [-]id, [-]createdAt, [-]popularity
    /// </summary>
    public string OrderBy
    {
      get
      {
        return _orderBy;
      }
      set
      {
        __isset.@orderBy = true;
        this._orderBy = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sessionId;
      public bool pagination;
      public bool searchTerm;
      public bool followedByUserId;
      public bool memberUserId;
      public bool isTrending;
      public bool @orderBy;
    }

    public GetGroupsRequest() {
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
                Pagination = new Pagination();
                Pagination.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                SearchTerm = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                FollowedByUserId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.String) {
                MemberUserId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 7:
              if (field.Type == TType.Bool) {
                IsTrending = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.String) {
                OrderBy = iprot.ReadString();
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
        TStruct struc = new TStruct("GetGroupsRequest");
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
        if (Pagination != null && __isset.pagination) {
          field.Name = "pagination";
          field.Type = TType.Struct;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          Pagination.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (SearchTerm != null && __isset.searchTerm) {
          field.Name = "searchTerm";
          field.Type = TType.String;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(SearchTerm);
          oprot.WriteFieldEnd();
        }
        if (FollowedByUserId != null && __isset.followedByUserId) {
          field.Name = "followedByUserId";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(FollowedByUserId);
          oprot.WriteFieldEnd();
        }
        if (MemberUserId != null && __isset.memberUserId) {
          field.Name = "memberUserId";
          field.Type = TType.String;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(MemberUserId);
          oprot.WriteFieldEnd();
        }
        if (OrderBy != null && __isset.@orderBy) {
          field.Name = "orderBy";
          field.Type = TType.String;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(OrderBy);
          oprot.WriteFieldEnd();
        }
        if (__isset.isTrending) {
          field.Name = "isTrending";
          field.Type = TType.Bool;
          field.ID = 7;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(IsTrending);
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
      StringBuilder __sb = new StringBuilder("GetGroupsRequest(");
      bool __first = true;
      if (SessionId != null && __isset.sessionId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("SessionId: ");
        __sb.Append(SessionId);
      }
      if (Pagination != null && __isset.pagination) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Pagination: ");
        __sb.Append(Pagination== null ? "<null>" : Pagination.ToString());
      }
      if (SearchTerm != null && __isset.searchTerm) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("SearchTerm: ");
        __sb.Append(SearchTerm);
      }
      if (FollowedByUserId != null && __isset.followedByUserId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("FollowedByUserId: ");
        __sb.Append(FollowedByUserId);
      }
      if (MemberUserId != null && __isset.memberUserId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("MemberUserId: ");
        __sb.Append(MemberUserId);
      }
      if (__isset.isTrending) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("IsTrending: ");
        __sb.Append(IsTrending);
      }
      if (OrderBy != null && __isset.@orderBy) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("OrderBy: ");
        __sb.Append(OrderBy);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
