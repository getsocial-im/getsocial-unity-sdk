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
  public partial class GetAnnouncementsResponse : TBase
  {
    private List<AFAnnouncement> _data;
    private List<AFEntityReference> _entityDetails;
    private Dictionary<string, THPublicUser> _authors;
    private string _nextCursor;

    public List<AFAnnouncement> Data
    {
      get
      {
        return _data;
      }
      set
      {
        __isset.data = true;
        this._data = value;
      }
    }

    public List<AFEntityReference> EntityDetails
    {
      get
      {
        return _entityDetails;
      }
      set
      {
        __isset.entityDetails = true;
        this._entityDetails = value;
      }
    }

    public Dictionary<string, THPublicUser> Authors
    {
      get
      {
        return _authors;
      }
      set
      {
        __isset.authors = true;
        this._authors = value;
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
      public bool data;
      public bool entityDetails;
      public bool authors;
      public bool nextCursor;
    }

    public GetAnnouncementsResponse() {
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
                  Data = new List<AFAnnouncement>();
                  TList _list127 = iprot.ReadListBegin();
                  for( int _i128 = 0; _i128 < _list127.Count; ++_i128)
                  {
                    AFAnnouncement _elem129;
                    _elem129 = new AFAnnouncement();
                    _elem129.Read(iprot);
                    Data.Add(_elem129);
                  }
                  iprot.ReadListEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.List) {
                {
                  EntityDetails = new List<AFEntityReference>();
                  TList _list130 = iprot.ReadListBegin();
                  for( int _i131 = 0; _i131 < _list130.Count; ++_i131)
                  {
                    AFEntityReference _elem132;
                    _elem132 = new AFEntityReference();
                    _elem132.Read(iprot);
                    EntityDetails.Add(_elem132);
                  }
                  iprot.ReadListEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.Map) {
                {
                  Authors = new Dictionary<string, THPublicUser>();
                  TMap _map133 = iprot.ReadMapBegin();
                  for( int _i134 = 0; _i134 < _map133.Count; ++_i134)
                  {
                    string _key135;
                    THPublicUser _val136;
                    _key135 = iprot.ReadString();
                    _val136 = new THPublicUser();
                    _val136.Read(iprot);
                    Authors[_key135] = _val136;
                  }
                  iprot.ReadMapEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
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
        TStruct struc = new TStruct("GetAnnouncementsResponse");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Data != null && __isset.data) {
          field.Name = "data";
          field.Type = TType.List;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.Struct, Data.Count));
            foreach (AFAnnouncement _iter137 in Data)
            {
              _iter137.Write(oprot);
            }
            oprot.WriteListEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (EntityDetails != null && __isset.entityDetails) {
          field.Name = "entityDetails";
          field.Type = TType.List;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.Struct, EntityDetails.Count));
            foreach (AFEntityReference _iter138 in EntityDetails)
            {
              _iter138.Write(oprot);
            }
            oprot.WriteListEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (Authors != null && __isset.authors) {
          field.Name = "authors";
          field.Type = TType.Map;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteMapBegin(new TMap(TType.String, TType.Struct, Authors.Count));
            foreach (string _iter139 in Authors.Keys)
            {
              oprot.WriteString(_iter139);
              Authors[_iter139].Write(oprot);
            }
            oprot.WriteMapEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (NextCursor != null && __isset.nextCursor) {
          field.Name = "nextCursor";
          field.Type = TType.String;
          field.ID = 4;
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
      StringBuilder __sb = new StringBuilder("GetAnnouncementsResponse(");
      bool __first = true;
      if (Data != null && __isset.data) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Data: ");
        __sb.Append(Data.ToDebugString());
      }
      if (EntityDetails != null && __isset.entityDetails) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("EntityDetails: ");
        __sb.Append(EntityDetails.ToDebugString());
      }
      if (Authors != null && __isset.authors) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Authors: ");
        __sb.Append(Authors.ToDebugString());
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
