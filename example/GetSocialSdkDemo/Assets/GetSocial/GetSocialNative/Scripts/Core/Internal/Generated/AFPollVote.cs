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
  public partial class AFPollVote : TBase
  {
    private THCreator _creator;
    private List<string> _optionIds;

    public THCreator Creator
    {
      get
      {
        return _creator;
      }
      set
      {
        __isset.creator = true;
        this._creator = value;
      }
    }

    public List<string> OptionIds
    {
      get
      {
        return _optionIds;
      }
      set
      {
        __isset.optionIds = true;
        this._optionIds = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool creator;
      public bool optionIds;
    }

    public AFPollVote() {
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
                Creator = new THCreator();
                Creator.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.List) {
                {
                  OptionIds = new List<string>();
                  TList _list17 = iprot.ReadListBegin();
                  for( int _i18 = 0; _i18 < _list17.Count; ++_i18)
                  {
                    string _elem19;
                    _elem19 = iprot.ReadString();
                    OptionIds.Add(_elem19);
                  }
                  iprot.ReadListEnd();
                }
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
        TStruct struc = new TStruct("AFPollVote");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Creator != null && __isset.creator) {
          field.Name = "creator";
          field.Type = TType.Struct;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          Creator.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (OptionIds != null && __isset.optionIds) {
          field.Name = "optionIds";
          field.Type = TType.List;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.String, OptionIds.Count));
            foreach (string _iter20 in OptionIds)
            {
              oprot.WriteString(_iter20);
            }
            oprot.WriteListEnd();
          }
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
      StringBuilder __sb = new StringBuilder("AFPollVote(");
      bool __first = true;
      if (Creator != null && __isset.creator) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Creator: ");
        __sb.Append(Creator== null ? "<null>" : Creator.ToString());
      }
      if (OptionIds != null && __isset.optionIds) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("OptionIds: ");
        __sb.Append(OptionIds.ToDebugString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif