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
  /// and will use that information to read the correct field from Field.
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class Field : TBase
  {
    private long _intVal;
    private double _dVal;
    private string _dateVal;
    private string _strVal;
    private bool _boolVal;

    public long IntVal
    {
      get
      {
        return _intVal;
      }
      set
      {
        __isset.intVal = true;
        this._intVal = value;
      }
    }

    public double DVal
    {
      get
      {
        return _dVal;
      }
      set
      {
        __isset.dVal = true;
        this._dVal = value;
      }
    }

    public string DateVal
    {
      get
      {
        return _dateVal;
      }
      set
      {
        __isset.dateVal = true;
        this._dateVal = value;
      }
    }

    public string StrVal
    {
      get
      {
        return _strVal;
      }
      set
      {
        __isset.strVal = true;
        this._strVal = value;
      }
    }

    public bool BoolVal
    {
      get
      {
        return _boolVal;
      }
      set
      {
        __isset.boolVal = true;
        this._boolVal = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool intVal;
      public bool dVal;
      public bool dateVal;
      public bool strVal;
      public bool boolVal;
    }

    public Field() {
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
              if (field.Type == TType.I64) {
                IntVal = iprot.ReadI64();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.Double) {
                DVal = iprot.ReadDouble();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                DateVal = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                StrVal = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.Bool) {
                BoolVal = iprot.ReadBool();
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
        TStruct struc = new TStruct("Field");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (__isset.intVal) {
          field.Name = "intVal";
          field.Type = TType.I64;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(IntVal);
          oprot.WriteFieldEnd();
        }
        if (__isset.dVal) {
          field.Name = "dVal";
          field.Type = TType.Double;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteDouble(DVal);
          oprot.WriteFieldEnd();
        }
        if (DateVal != null && __isset.dateVal) {
          field.Name = "dateVal";
          field.Type = TType.String;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(DateVal);
          oprot.WriteFieldEnd();
        }
        if (StrVal != null && __isset.strVal) {
          field.Name = "strVal";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(StrVal);
          oprot.WriteFieldEnd();
        }
        if (__isset.boolVal) {
          field.Name = "boolVal";
          field.Type = TType.Bool;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(BoolVal);
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
      StringBuilder __sb = new StringBuilder("Field(");
      bool __first = true;
      if (__isset.intVal) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("IntVal: ");
        __sb.Append(IntVal);
      }
      if (__isset.dVal) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("DVal: ");
        __sb.Append(DVal);
      }
      if (DateVal != null && __isset.dateVal) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("DateVal: ");
        __sb.Append(DateVal);
      }
      if (StrVal != null && __isset.strVal) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("StrVal: ");
        __sb.Append(StrVal);
      }
      if (__isset.boolVal) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("BoolVal: ");
        __sb.Append(BoolVal);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
