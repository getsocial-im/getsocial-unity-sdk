using System.Linq;
using System.Reflection;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.MiniJSON
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;

    // Example usage:
    //
    //  using UnityEngine;
    //  using System.Collections;
    //  using System.Collections.Generic;
    //  using MiniJSON;
    //
    //  public class MiniJSONTest : MonoBehaviour {
    //      void Start () {
    //          var jsonString = "{ \"array\": [1.44,2,3], " +
    //                          "\"object\": {\"key1\":\"value1\", \"key2\":256}, " +
    //                          "\"string\": \"The quick brown fox \\\"jumps\\\" over the lazy dog \", " +
    //                          "\"unicode\": \"\\u3041 Men\u00fa sesi\u00f3n\", " +
    //                          "\"int\": 65536, " +
    //                          "\"float\": 3.1415926, " +
    //                          "\"bool\": true, " +
    //                          "\"null\": null }";
    //
    //          var dict = Json.Deserialize(jsonString) as Dictionary<string,object>;
    //
    //          Debug.Log("deserialized: " + dict.GetType());
    //          Debug.Log("dict['array'][0]: " + ((List<object>) dict["array"])[0]);
    //          Debug.Log("dict['string']: " + (string) dict["string"]);
    //          Debug.Log("dict['float']: " + (double) dict["float"]); // floats come out as doubles
    //          Debug.Log("dict['int']: " + (long) dict["int"]); // ints come out as longs
    //          Debug.Log("dict['unicode']: " + (string) dict["unicode"]);
    //
    //          var str = Json.Serialize(dict);
    //
    //          Debug.Log("serialized: " + str);
    //      }
    //  }

    /// <summary>
    /// This class encodes and decodes JSON strings.
    /// Spec. details, see http://www.json.org/
    ///
    /// JSON uses Arrays and Objects. These correspond here to the datatypes IList and IDictionary.
    /// All numbers are parsed to doubles.
    /// </summary>
    public static class GSJson
    {
        // interpret all numbers as if they are english US formatted numbers
        private static NumberFormatInfo numberFormat = (new CultureInfo("en-US")).NumberFormat;

        /// <summary>
        /// Parses the string json into a value
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An List&lt;object&gt;, a Dictionary&lt;string, object&gt;, a double, an integer,a string, null, true, or false</returns>
        public static object Deserialize(string json)
        {
            // save the string for debug information
            if (json == null)
            {
                return null;
            }

            if (json.Length == 0)
            {
                return null;
            }

            return Parser.Parse(json);
        }

        /// <summary>
        /// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
        /// </summary>
        /// <param name="json">A Dictionary&lt;string, object&gt; / List&lt;object&gt;</param>
        /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
        public static string Serialize(object obj)
        {
            return Serializer.Serialize(obj);
        }

        public static string TextureToBase64(this Texture2D obj)
        {
            if (obj == null)
            {
                return null;
            }
            var bytes = obj.EncodeToPNG();
            if (bytes == null)
            {
                return null;
            }
            return Convert.ToBase64String(bytes);
        }

        public static Texture2D FromBase64(this string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image))
            {
                return null;
            }

            var b64_bytes = Convert.FromBase64String(base64Image);
            var tex = new Texture2D(1,1);
            tex.LoadImage(b64_bytes);
            tex.Apply();
            return tex;
        }
        
        public static string ByteArrayToBase64(this byte[] byteArray)
        {
            if (byteArray == null)
            {
                return "";
            }
            return Convert.ToBase64String(byteArray);
        }

        public static T ToObject<T>(object json)
        {
            var obj = ToObject(json, typeof(T));
            return (T) obj;
        }

        private static object ToObject(object json, Type type)
        {
            if (json == null || type == typeof(string))
            {
                return json;
            }

            if (type.IsPrimitive)
            {
                if (type == typeof(bool))
                {
                    return json;
                }

                return Convert.ChangeType(json, type);
            }

            if (type.IsEnum)
            {
                return Enum.ToObject(type, Convert.ChangeType(json, typeof(int)));
            }

            if (type.IsGenericList())
            {
                var listType = typeof(List<>);
                var genericType = type.GetGenericArguments()[0];
                var constructedListType = listType.MakeGenericType(genericType);

                var instance = (IList) Activator.CreateInstance(constructedListType);
                foreach (var item in (List<object>) json)
                {
                    instance.Add(GSJson.ToObject(item, genericType));
                }

                return instance;
            }

            if (type.IsGenericDictionary())
            {
                var dictionaryType = typeof(Dictionary<,>);
                var keyType = type.GetGenericArguments()[0];
                var valueType = type.GetGenericArguments()[1];
                
                var constructedDictionaryType = dictionaryType.MakeGenericType(keyType, valueType);

                var instance = (IDictionary) Activator.CreateInstance(constructedDictionaryType);
                foreach (var item in (Dictionary<string, object>) json)
                {
                    var key = GSJson.ToObject(item.Key, keyType);
                    instance[key] = GSJson.ToObject(item.Value, valueType);
                }

                return instance;
            }

            var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] {}, null);
            if (constructor == null)
            {
                constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[] {}, null);
            }
            var res = constructor.Invoke(new object[]{});
            var dictionary = (Dictionary<string, object>) json;
            Action<FieldInfo> attrField = field =>
            {
                var attrs = (JsonSerializationKey[]) field.GetCustomAttributes
                    (typeof(JsonSerializationKey), false);
                if (attrs.Length != 0)
                {
                    var value = GSJson.ToObject(
                        dictionary.ContainsKey(attrs[0].Name) ? dictionary[attrs[0].Name] : null, field.FieldType);
                    field.SetValue(res, value);
                }
            };
            Action<PropertyInfo> attrProperty = property =>
            {
                var attrs = (JsonSerializationKey[]) property.GetCustomAttributes
                    (typeof(JsonSerializationKey), false);
                if (attrs.Length != 0)
                {
                    var value = GSJson.ToObject(
                        dictionary.ContainsKey(attrs[0].Name) ? dictionary[attrs[0].Name] : null, property.PropertyType);
                    property.SetValue(res, value);
                }
            };
            type.GetFields().ToList().ForEach(attrField);
            type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).ToList().ForEach(attrField);
            type.GetProperties().ToList().ForEach(attrProperty);
            type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).ToList().ForEach(attrProperty);

            return res;
        }
        
        private sealed class Parser : IDisposable
        {
            private const string WhiteSpace = " \t\n\r";
            private const string WordBreak = " \t\n\r{}[],:\"";

            private StringReader json;

            private Parser(string jsonString)
            {
                this.json = new StringReader(jsonString);
            }

            private enum TOKEN
            {
                NONE,
                CURLY_OPEN,
                CURLY_CLOSE,
                SQUARED_OPEN,
                SQUARED_CLOSE,
                COLON,
                COMMA,
                STRING,
                NUMBER,
                TRUE,
                FALSE,
                NULL
            }

            private char PeekChar
            {
                get
                {
                    return Convert.ToChar(this.json.Peek());
                }
            }

            private char NextChar
            {
                get
                {
                    return Convert.ToChar(this.json.Read());
                }
            }

            private string NextWord
            {
                get
                {
                    StringBuilder word = new StringBuilder();

                    while (WordBreak.IndexOf(this.PeekChar) == -1)
                    {
                        word.Append(this.NextChar);

                        if (this.json.Peek() == -1)
                        {
                            break;
                        }
                    }

                    return word.ToString();
                }
            }

            private TOKEN NextToken
            {
                get
                {
                    this.EatWhitespace();

                    if (this.json.Peek() == -1)
                    {
                        return TOKEN.NONE;
                    }

                    char c = this.PeekChar;
                    switch (c)
                    {
                        case '{':
                            return TOKEN.CURLY_OPEN;
                        case '}':
                            this.json.Read();
                            return TOKEN.CURLY_CLOSE;
                        case '[':
                            return TOKEN.SQUARED_OPEN;
                        case ']':
                            this.json.Read();
                            return TOKEN.SQUARED_CLOSE;
                        case ',':
                            this.json.Read();
                            return TOKEN.COMMA;
                        case '"':
                            return TOKEN.STRING;
                        case ':':
                            return TOKEN.COLON;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case '-':
                            return TOKEN.NUMBER;
                    }

                    string word = this.NextWord;

                    switch (word)
                    {
                        case "false":
                            return TOKEN.FALSE;
                        case "true":
                            return TOKEN.TRUE;
                        case "null":
                            return TOKEN.NULL;
                    }

                    return TOKEN.NONE;
                }
            }

            public static object Parse(string jsonString)
            {
                using (var instance = new Parser(jsonString))
                {
                    return instance.ParseValue();
                }
            }

            public void Dispose()
            {
                this.json.Dispose();
                this.json = null;
            }

            private Dictionary<string, object> ParseObject()
            {
                Dictionary<string, object> table = new Dictionary<string, object>();

                // ditch opening brace
                this.json.Read();

                // {
                while (true)
                {
                    switch (this.NextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.CURLY_CLOSE:
                            return table;
                        default:
                            // name
                            string name = this.ParseString();
                            if (name == null)
                            {
                                return null;
                            }

                            // :
                            if (this.NextToken != TOKEN.COLON)
                            {
                                return null;
                            }

                            // ditch the colon
                            this.json.Read();

                            // value
                            table[name] = this.ParseValue();
                            break;
                    }
                }
            }

            private List<object> ParseArray()
            {
                List<object> array = new List<object>();

                // ditch opening bracket
                this.json.Read();

                // [
                var parsing = true;
                while (parsing)
                {
                    TOKEN nextToken = this.NextToken;

                    switch (nextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.SQUARED_CLOSE:
                            parsing = false;
                            break;
                        default:
                            object value = this.ParseByToken(nextToken);

                            array.Add(value);
                            break;
                    }
                }

                return array;
            }

            private object ParseValue()
            {
                TOKEN nextToken = this.NextToken;
                return this.ParseByToken(nextToken);
            }

            private object ParseByToken(TOKEN token)
            {
                switch (token)
                {
                    case TOKEN.STRING:
                        return this.ParseString();
                    case TOKEN.NUMBER:
                        return this.ParseNumber();
                    case TOKEN.CURLY_OPEN:
                        return this.ParseObject();
                    case TOKEN.SQUARED_OPEN:
                        return this.ParseArray();
                    case TOKEN.TRUE:
                        return true;
                    case TOKEN.FALSE:
                        return false;
                    case TOKEN.NULL:
                        return null;
                    default:
                        return null;
                }
            }

            private string ParseString()
            {
                StringBuilder s = new StringBuilder();
                char c;

                // ditch opening quote
                this.json.Read();

                bool parsing = true;
                while (parsing)
                {
                    if (this.json.Peek() == -1)
                    {
                        parsing = false;
                        break;
                    }

                    c = this.NextChar;
                    switch (c)
                    {
                        case '"':
                            parsing = false;
                            break;
                        case '\\':
                            if (this.json.Peek() == -1)
                            {
                                parsing = false;
                                break;
                            }

                            c = this.NextChar;
                            switch (c)
                            {
                                case '"':
                                case '\\':
                                case '/':
                                    s.Append(c);
                                    break;
                                case 'b':
                                    s.Append('\b');
                                    break;
                                case 'f':
                                    s.Append('\f');
                                    break;
                                case 'n':
                                    s.Append('\n');
                                    break;
                                case 'r':
                                    s.Append('\r');
                                    break;
                                case 't':
                                    s.Append('\t');
                                    break;
                                case 'u':
                                    var hex = new StringBuilder();

                                    for (int i = 0; i < 4; i++)
                                    {
                                        hex.Append(this.NextChar);
                                    }

                                    s.Append((char)Convert.ToInt32(hex.ToString(), 16));
                                    break;
                            }

                            break;
                        default:
                            s.Append(c);
                            break;
                    }
                }

                return s.ToString();
            }

            private object ParseNumber()
            {
                string number = this.NextWord;

                if (number.IndexOf('.') == -1)
                {
                    return long.Parse(number, numberFormat);
                }

                return double.Parse(number, numberFormat);
            }

            private void EatWhitespace()
            {
                while (WhiteSpace.IndexOf(this.PeekChar) != -1)
                {
                    this.json.Read();

                    if (this.json.Peek() == -1)
                    {
                        break;
                    }
                }
            }
        }

        private sealed class Serializer
        {
            private StringBuilder builder;

            private Serializer()
            {
                this.builder = new StringBuilder();
            }

            public static string Serialize(object obj)
            {
                var instance = new Serializer();

                instance.SerializeValue(obj);

                return instance.builder.ToString();
            }

            private void SerializeValue(object value)
            {
                IList asList;
                IDictionary asDict;
                string asStr;

                if (value == null)
                {
                    this.builder.Append("null");
                }
                else if ((asStr = value as string) != null)
                {
                    this.SerializeString(asStr);
                }
                else if (value is bool)
                {
                    this.builder.Append(value.ToString().ToLower());
                }
                else if ((asList = value as IList) != null)
                {
                    this.SerializeArray(asList);
                }
                else if ((asDict = value as IDictionary) != null)
                {
                    this.SerializeObject(asDict);
                }
                else if (value is char)
                {
                    this.SerializeString(value.ToString());
                }
                else
                {
                    this.SerializeOther(value);
                }
            }

            private void SerializeObject(IDictionary obj)
            {
                bool first = true;

                this.builder.Append('{');

                foreach (object e in obj.Keys)
                {
                    if (!first)
                    {
                        this.builder.Append(',');
                    }

                    this.SerializeString(e.ToString());
                    this.builder.Append(':');

                    this.SerializeValue(obj[e]);

                    first = false;
                }

                this.builder.Append('}');
            }

            private void SerializeArray(IList array)
            {
                this.builder.Append('[');

                bool first = true;

                foreach (object obj in array)
                {
                    if (!first)
                    {
                        this.builder.Append(',');
                    }

                    this.SerializeValue(obj);

                    first = false;
                }

                this.builder.Append(']');
            }

            private void SerializeString(string str)
            {
                this.builder.Append('\"');

                char[] charArray = str.ToCharArray();
                foreach (var c in charArray)
                {
                    switch (c)
                    {
                        case '"':
                            this.builder.Append("\\\"");
                            break;
                        case '\\':
                            this.builder.Append("\\\\");
                            break;
                        case '\b':
                            this.builder.Append("\\b");
                            break;
                        case '\f':
                            this.builder.Append("\\f");
                            break;
                        case '\n':
                            this.builder.Append("\\n");
                            break;
                        case '\r':
                            this.builder.Append("\\r");
                            break;
                        case '\t':
                            this.builder.Append("\\t");
                            break;
                        default:
                            int codepoint = Convert.ToInt32(c);
                            if ((codepoint >= 32) && (codepoint <= 126))
                            {
                                this.builder.Append(c);
                            }
                            else
                            {
                                this.builder.Append("\\u" + Convert.ToString(codepoint, 16).PadLeft(4, '0'));
                            }

                            break;
                    }
                }

                this.builder.Append('\"');
            }

            private void SerializeOther(object value)
            {
                if (value is float
                    || value is int
                    || value is uint
                    || value is long
                    || value is double
                    || value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is ulong
                    || value is decimal)
                {
                    this.builder.Append(value.ToString());
                }
                else if (value.GetType().IsEnum)
                {
                    this.builder.Append((int) value);
                }
                else {
                    var toJson = value.GetType().GetMethod("ToJson", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (toJson == null)
                    {
                        this.SerializeObject(ToDictionary(value));
                    }
                    else
                    {
                        this.SerializeOther(toJson.Invoke(value, new object[]{}));
                    }
                }
            }

            private static IDictionary ToDictionary(object value)
            {
                var type = value.GetType();
                var dictionary = new SortedDictionary<string, object>();
                Action<FieldInfo> attrField = field =>
                {
                    var attrs = (JsonSerializationKey[]) field.GetCustomAttributes
                        (typeof(JsonSerializationKey), false);
                    foreach (var attr in attrs)
                    {
                        dictionary[attr.Name] = field.GetValue(value);
                    }
                };
                Action<PropertyInfo> attrProperty = field =>
                {
                    var attrs = (JsonSerializationKey[]) field.GetCustomAttributes
                        (typeof(JsonSerializationKey), false);
                    foreach (var attr in attrs)
                    {
                        dictionary[attr.Name] = field.GetValue(value);
                    }
                };
                type.GetFields().ToList().ForEach(attrField);
                type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).ToList().ForEach(attrField);
                type.GetProperties().ToList().ForEach(attrProperty);
                type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).ToList().ForEach(attrProperty);

                return dictionary;
            }


        }
        public static bool IsGenericList(this Type oType)
        {
            return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(List<>)));
        }
        public static bool IsGenericDictionary(this Type oType)
        {
            return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(Dictionary<,>)));
        }
    }
}