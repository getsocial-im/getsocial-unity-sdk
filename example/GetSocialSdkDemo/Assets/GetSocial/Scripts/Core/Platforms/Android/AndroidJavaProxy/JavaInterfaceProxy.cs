#if UNITY_ANDROID
using System;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public class JavaInterfaceProxy : AndroidJavaProxy
    {
        static JavaInterfaceProxy _comparingWho;

        internal JavaInterfaceProxy(string javaInterface)
            : base(javaInterface)
        {
        }

        /// <summary>
        /// Unity forwards all calls from Java to C# including call to equals(), so we have to override it.
        /// More about issue: http://forum.unity3d.com/threads/androidjavaproxy-equals.243438/
        /// </summary>
        protected bool equals(AndroidJavaObject other)
        {
            bool result;

            if (_comparingWho != null)
            {
                result = _comparingWho == this;
                _comparingWho = null;
            }
            else
            {
                _comparingWho = this;
                result = other.Call<bool>("equals", other);
            }

            return result;
        }

        protected string toString()
        {
            return GetType().Name;
        }

        protected void ExecuteOnMainThread(Action action)
        {
            MainThreadExecutor.Queue(action);
        }
    }
}
#endif