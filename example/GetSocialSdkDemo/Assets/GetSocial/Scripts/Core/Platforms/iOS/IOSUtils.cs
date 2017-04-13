#if UNITY_IOS
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GetSocialSdk.Core
{
    static class IOSUtils
    {
        public static IntPtr GetPointer(this object obj)
        {
            return obj == null ? IntPtr.Zero : GCHandle.ToIntPtr(GCHandle.Alloc(obj));
        }

        public static T Cast<T>(this IntPtr instancePtr)
        {
            var instanceHandle = GCHandle.FromIntPtr(instancePtr);
            if (!(instanceHandle.Target is T)) throw new InvalidCastException("Failed to cast IntPtr");

            var castedTarget = (T) instanceHandle.Target;
            return castedTarget;
        }

        public static void TriggerCallback<T>(IntPtr actionPtr, T result)
        {
            if (actionPtr != IntPtr.Zero)
            {
                actionPtr.Cast<Action<T>>().Invoke(result);
            }
        }

        public static Texture2D FromBase64(string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image))
            {
                return null;
            }

            var b64_bytes = Convert.FromBase64String(base64Image);
            var tex = new Texture2D(1,1);
            tex.LoadImage(b64_bytes);
            return tex;
        }
    }
}

#endif