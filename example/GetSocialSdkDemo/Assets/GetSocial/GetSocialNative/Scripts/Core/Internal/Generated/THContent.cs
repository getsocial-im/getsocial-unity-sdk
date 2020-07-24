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
  /// #sdk6
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class THContent : TBase
  {
    private string _text;
    private string _imageUrl;
    private THButton _button;
    private string _videoUrl;

    public string Text
    {
      get
      {
        return _text;
      }
      set
      {
        __isset.text = true;
        this._text = value;
      }
    }

    public string ImageUrl
    {
      get
      {
        return _imageUrl;
      }
      set
      {
        __isset.imageUrl = true;
        this._imageUrl = value;
      }
    }

    public THButton Button
    {
      get
      {
        return _button;
      }
      set
      {
        __isset.button = true;
        this._button = value;
      }
    }

    public string VideoUrl
    {
      get
      {
        return _videoUrl;
      }
      set
      {
        __isset.videoUrl = true;
        this._videoUrl = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool text;
      public bool imageUrl;
      public bool button;
      public bool videoUrl;
    }

    public THContent() {
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
                Text = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                ImageUrl = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.Struct) {
                Button = new THButton();
                Button.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                VideoUrl = iprot.ReadString();
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
        TStruct struc = new TStruct("THContent");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Text != null && __isset.text) {
          field.Name = "text";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Text);
          oprot.WriteFieldEnd();
        }
        if (ImageUrl != null && __isset.imageUrl) {
          field.Name = "imageUrl";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(ImageUrl);
          oprot.WriteFieldEnd();
        }
        if (Button != null && __isset.button) {
          field.Name = "button";
          field.Type = TType.Struct;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          Button.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (VideoUrl != null && __isset.videoUrl) {
          field.Name = "videoUrl";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(VideoUrl);
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
      StringBuilder __sb = new StringBuilder("THContent(");
      bool __first = true;
      if (Text != null && __isset.text) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Text: ");
        __sb.Append(Text);
      }
      if (ImageUrl != null && __isset.imageUrl) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ImageUrl: ");
        __sb.Append(ImageUrl);
      }
      if (Button != null && __isset.button) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Button: ");
        __sb.Append(Button== null ? "<null>" : Button.ToString());
      }
      if (VideoUrl != null && __isset.videoUrl) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("VideoUrl: ");
        __sb.Append(VideoUrl);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
