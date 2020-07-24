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
  public partial class SGGroup : TBase
  {
    private string _id;
    private Dictionary<string, string> _title;
    private Dictionary<string, string> _groupDescription;
    private string _avatarUrl;
    private long _createdAt;
    private long _updatedAt;
    private string _createdBy;
    private SGSettings _settings;
    private int _followersCount;
    private bool _isFollower;
    private int _membersCount;
    private SGMembershipInfo _membership;

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

    public Dictionary<string, string> Title
    {
      get
      {
        return _title;
      }
      set
      {
        __isset.title = true;
        this._title = value;
      }
    }

    public Dictionary<string, string> GroupDescription
    {
      get
      {
        return _groupDescription;
      }
      set
      {
        __isset.groupDescription = true;
        this._groupDescription = value;
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

    public long UpdatedAt
    {
      get
      {
        return _updatedAt;
      }
      set
      {
        __isset.updatedAt = true;
        this._updatedAt = value;
      }
    }

    /// <summary>
    /// user id of creator
    /// </summary>
    public string CreatedBy
    {
      get
      {
        return _createdBy;
      }
      set
      {
        __isset.createdBy = true;
        this._createdBy = value;
      }
    }

    public SGSettings Settings
    {
      get
      {
        return _settings;
      }
      set
      {
        __isset.settings = true;
        this._settings = value;
      }
    }

    public int FollowersCount
    {
      get
      {
        return _followersCount;
      }
      set
      {
        __isset.followersCount = true;
        this._followersCount = value;
      }
    }

    public bool IsFollower
    {
      get
      {
        return _isFollower;
      }
      set
      {
        __isset.isFollower = true;
        this._isFollower = value;
      }
    }

    public int MembersCount
    {
      get
      {
        return _membersCount;
      }
      set
      {
        __isset.membersCount = true;
        this._membersCount = value;
      }
    }

    /// <summary>
    /// Not include pending members
    /// </summary>
    public SGMembershipInfo Membership
    {
      get
      {
        return _membership;
      }
      set
      {
        __isset.membership = true;
        this._membership = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool id;
      public bool title;
      public bool groupDescription;
      public bool avatarUrl;
      public bool createdAt;
      public bool updatedAt;
      public bool createdBy;
      public bool settings;
      public bool followersCount;
      public bool isFollower;
      public bool membersCount;
      public bool membership;
    }

    public SGGroup() {
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
              if (field.Type == TType.Map) {
                {
                  Title = new Dictionary<string, string>();
                  TMap _map0 = iprot.ReadMapBegin();
                  for( int _i1 = 0; _i1 < _map0.Count; ++_i1)
                  {
                    string _key2;
                    string _val3;
                    _key2 = iprot.ReadString();
                    _val3 = iprot.ReadString();
                    Title[_key2] = _val3;
                  }
                  iprot.ReadMapEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.Map) {
                {
                  GroupDescription = new Dictionary<string, string>();
                  TMap _map4 = iprot.ReadMapBegin();
                  for( int _i5 = 0; _i5 < _map4.Count; ++_i5)
                  {
                    string _key6;
                    string _val7;
                    _key6 = iprot.ReadString();
                    _val7 = iprot.ReadString();
                    GroupDescription[_key6] = _val7;
                  }
                  iprot.ReadMapEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                AvatarUrl = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.I64) {
                CreatedAt = iprot.ReadI64();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.I64) {
                UpdatedAt = iprot.ReadI64();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 7:
              if (field.Type == TType.String) {
                CreatedBy = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 8:
              if (field.Type == TType.Struct) {
                Settings = new SGSettings();
                Settings.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 9:
              if (field.Type == TType.I32) {
                FollowersCount = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 10:
              if (field.Type == TType.Bool) {
                IsFollower = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 11:
              if (field.Type == TType.I32) {
                MembersCount = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 12:
              if (field.Type == TType.Struct) {
                Membership = new SGMembershipInfo();
                Membership.Read(iprot);
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
        TStruct struc = new TStruct("SGGroup");
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
        if (Title != null && __isset.title) {
          field.Name = "title";
          field.Type = TType.Map;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteMapBegin(new TMap(TType.String, TType.String, Title.Count));
            foreach (string _iter8 in Title.Keys)
            {
              oprot.WriteString(_iter8);
              oprot.WriteString(Title[_iter8]);
            }
            oprot.WriteMapEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (GroupDescription != null && __isset.groupDescription) {
          field.Name = "groupDescription";
          field.Type = TType.Map;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteMapBegin(new TMap(TType.String, TType.String, GroupDescription.Count));
            foreach (string _iter9 in GroupDescription.Keys)
            {
              oprot.WriteString(_iter9);
              oprot.WriteString(GroupDescription[_iter9]);
            }
            oprot.WriteMapEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (AvatarUrl != null && __isset.avatarUrl) {
          field.Name = "avatarUrl";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(AvatarUrl);
          oprot.WriteFieldEnd();
        }
        if (__isset.createdAt) {
          field.Name = "createdAt";
          field.Type = TType.I64;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(CreatedAt);
          oprot.WriteFieldEnd();
        }
        if (__isset.updatedAt) {
          field.Name = "updatedAt";
          field.Type = TType.I64;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(UpdatedAt);
          oprot.WriteFieldEnd();
        }
        if (CreatedBy != null && __isset.createdBy) {
          field.Name = "createdBy";
          field.Type = TType.String;
          field.ID = 7;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(CreatedBy);
          oprot.WriteFieldEnd();
        }
        if (Settings != null && __isset.settings) {
          field.Name = "settings";
          field.Type = TType.Struct;
          field.ID = 8;
          oprot.WriteFieldBegin(field);
          Settings.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (__isset.followersCount) {
          field.Name = "followersCount";
          field.Type = TType.I32;
          field.ID = 9;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(FollowersCount);
          oprot.WriteFieldEnd();
        }
        if (__isset.isFollower) {
          field.Name = "isFollower";
          field.Type = TType.Bool;
          field.ID = 10;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(IsFollower);
          oprot.WriteFieldEnd();
        }
        if (__isset.membersCount) {
          field.Name = "membersCount";
          field.Type = TType.I32;
          field.ID = 11;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(MembersCount);
          oprot.WriteFieldEnd();
        }
        if (Membership != null && __isset.membership) {
          field.Name = "membership";
          field.Type = TType.Struct;
          field.ID = 12;
          oprot.WriteFieldBegin(field);
          Membership.Write(oprot);
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
      StringBuilder __sb = new StringBuilder("SGGroup(");
      bool __first = true;
      if (Id != null && __isset.id) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Id: ");
        __sb.Append(Id);
      }
      if (Title != null && __isset.title) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Title: ");
        __sb.Append(Title.ToDebugString());
      }
      if (GroupDescription != null && __isset.groupDescription) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("GroupDescription: ");
        __sb.Append(GroupDescription.ToDebugString());
      }
      if (AvatarUrl != null && __isset.avatarUrl) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("AvatarUrl: ");
        __sb.Append(AvatarUrl);
      }
      if (__isset.createdAt) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("CreatedAt: ");
        __sb.Append(CreatedAt);
      }
      if (__isset.updatedAt) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("UpdatedAt: ");
        __sb.Append(UpdatedAt);
      }
      if (CreatedBy != null && __isset.createdBy) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("CreatedBy: ");
        __sb.Append(CreatedBy);
      }
      if (Settings != null && __isset.settings) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Settings: ");
        __sb.Append(Settings== null ? "<null>" : Settings.ToString());
      }
      if (__isset.followersCount) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("FollowersCount: ");
        __sb.Append(FollowersCount);
      }
      if (__isset.isFollower) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("IsFollower: ");
        __sb.Append(IsFollower);
      }
      if (__isset.membersCount) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("MembersCount: ");
        __sb.Append(MembersCount);
      }
      if (Membership != null && __isset.membership) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Membership: ");
        __sb.Append(Membership== null ? "<null>" : Membership.ToString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
