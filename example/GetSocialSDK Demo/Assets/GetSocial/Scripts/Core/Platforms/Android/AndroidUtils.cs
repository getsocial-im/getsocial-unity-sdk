/**
 *     Copyright 2015-2016 GetSocial B.V.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#if UNITY_ANDROID

using UnityEngine;
using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    static class AndroidUtils
    {
        private const string JavaStringClass = "java.lang.String";

        private static AndroidJavaObject _activity;

        public static AndroidJavaObject Activity
        {
            get
            {
                if (_activity == null)
                {
                    var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    _activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                }
                return _activity;
            }
        }

        public static void RunOnUiThread(Action action)
        {
            Activity.Call("runOnUiThread", new AndroidJavaRunnable(action));
        }

        public static AndroidJavaObject ConvertToHashMap(IDictionary<string, string> stringStringDictionary)
        {
            if(stringStringDictionary == null)
            {
                return null;
            }
            
            var objHashMap = new AndroidJavaObject("java.util.HashMap");
            
            System.IntPtr methodPut = AndroidJNIHelper.GetMethodID(objHashMap.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
            
            object[] args = new object[2];
            foreach(KeyValuePair<string, string> keyValuePair in stringStringDictionary)
            {
                using(AndroidJavaObject javaStringKey = new AndroidJavaObject(JavaStringClass, keyValuePair.Key))
                {
                    using(AndroidJavaObject javaStringValue = new AndroidJavaObject(JavaStringClass, keyValuePair.Value))
                    {
                        args[0] = javaStringKey;
                        args[1] = javaStringValue;
                        AndroidJNI.CallObjectMethod(objHashMap.GetRawObject(), methodPut, AndroidJNIHelper.CreateJNIArgArray(args));
                    }
                }
            }
            
            return objHashMap;
        }

        public static AndroidJavaObject ConvertToArrayList(IList<string> stringList)
        {
            if(stringList == null)
            {
                return null;
            }
            
            var objArrayList = new AndroidJavaObject("java.util.ArrayList");
            
            for(int i = 0; i < stringList.Count; i++)
            {
                string value = stringList[i];
                using(AndroidJavaObject k = new AndroidJavaObject(JavaStringClass, value))
                {
                    objArrayList.Call("add", i, k);
                }
            }
            
            return objArrayList;
        }

        private const string BuildConfigClass = "im.getsocial.sdk.core.BuildConfig";

        public static bool IsUnityBuildOfGetSocialAndroidSDK()
        {
            using(AndroidJavaObject clazz = new AndroidJavaClass(BuildConfigClass))
            {
                var targetPlatform = clazz.GetStatic<AndroidJavaObject>("TARGET_PLATFORM");
                var targetPlatformString = targetPlatform.Call<string>("toString");

                return targetPlatformString.Equals("UNITY");
            }
        }

        public static string GetSdkEnvironment()
        {
            using(AndroidJavaObject clazz = new AndroidJavaClass(BuildConfigClass))
            {
                return clazz.GetStatic<string>("ENVIRONMENT");
            }
        }

        /// <summary>
        /// AndroidJavaObject is not smart enough to cast Java null to C# null. In other words Java null is valid AndroidJavaObject, 
        /// but when you'll try to invoke any method on it you'll end up with "A/libc﹕ Fatal signal 11 (SIGSEGV)" error in Logcat.
        /// Update: == operator is overloaded to check if it's Java null
        /// </summary>
        /// <returns><c>true</c> if AndroidJavaObject is null in Java; otherwise, <c>false</c>.</returns>
        /// <param name="javaObject">Java object.</param>
        public static bool IsJavaNull(this AndroidJavaObject javaObject)
        {
            return javaObject == null;
        }

        public static string MergeActivityTags(string[] tags)
        {
            if(tags == null || tags.Length == 0)
            {
                return null;
            }

            return string.Join(",", tags);
        }

        public static string GetRuntimeClassName(this AndroidJavaObject javaObj)
        {
            return javaObj.Call<AndroidJavaObject>("getClass").Call<string>("getName");
        }

        public static string GetSimpleClassName(this AndroidJavaObject javaObj)
        {
            return javaObj.Call<AndroidJavaObject>("getClass").Call<string>("getSimpleName");
        }

        public static string ToJavaString(this AndroidJavaObject javaObj)
        {
            return javaObj.Call<string>("toString");
        }

        #region parsing
        public static User UserFromJavaObj(AndroidJavaObject javaObjUser)
        {
            if (javaObjUser.GetSimpleClassName() != "User")
            {
                throw new System.InvalidOperationException("Trying to convert to user failed. This java object is not user");
            }

            var serializedUser = javaObjUser.Call<string>("serialize");

            javaObjUser.Dispose();
            return new User(new JSONObject(serializedUser));
        }

        public static System.DateTime DateTimeFromJavaUtilDate(AndroidJavaObject javaDate)
        {
            if (javaDate.GetSimpleClassName() != "Date")
            {
                throw new System.InvalidOperationException("Trying to convert to DateTime failed. This java object is not java date");
            }
            using (var simpleDateFormat = new AndroidJavaObject("java.text.SimpleDateFormat", ParseUtils.TimestampFormat))
            {
                string dateText = simpleDateFormat.Call<string>("format", javaDate);
                return ParseUtils.ParseTimestamp(dateText);
            }
        }
        #endregion

        public static string GetBitmapUri(byte[] data)
        {
            using (var gsUtilsClass = new AndroidJavaClass("im.getsocial.sdk.core.Utilities"))
            {
                var bitmap = Texture2DToAndroidBitmap(data);
                return gsUtilsClass.CallStatic<string>("saveAndGetImagePath", Activity, bitmap);
            }
        }

        public static AndroidJavaObject Texture2DToAndroidBitmap(byte[] data)
        {
            using (var bf = new AndroidJavaClass("android.graphics.BitmapFactory"))
            {
                return bf.CallStatic<AndroidJavaObject>("decodeByteArray", data, 0, data.Length);
            }
        }

        /**
         *    
         *    JNI method and constructor signature cheat sheet
         *
         *  B=byte
         *  C=char
         *  D=double
         *  F=float
         *  I=int
         *  J=long
         *  S=short
         *  V=void
         *  Z=boolean
         *  Lfully-qualified-class=fully qualified class
         *  [type=array of type>
         *  (argument types)return type=method type. 
         *     If no arguments, use empty argument types: (). 
         *     If return type is void (or constructor) use (argument types)V.*    
         *
         *     Example
         *     @code
         *     constructor:
         *     (String s)
         *
         *     translates to:
         *     (Ljava/lang/String;)V
         *
         *     method:
         *     String toString()
         *
         *     translates to:
         *     ()Ljava/lang/String;
         *
         *     method:
         *     long myMethod(int n, String s, int[] arr)
         *
         *     translates to:
         *     (ILjava/lang/String;[I)J
         *     @endcode
         *
         */
    }
}
#endif
