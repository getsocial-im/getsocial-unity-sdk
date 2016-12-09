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

using System.IO;
using UnityEngine;
using System;

#if UNITY_IOS
using System.Runtime.InteropServices;

namespace GetSocialSdk.Core
{
    internal class ConfigurationIOS : IConfiguration
    {
        private static IConfiguration instance = new ConfigurationIOS();

        #region initilization
        private ConfigurationIOS()
        {
        }

        internal static IConfiguration GetInstance()
        {
            return instance;
        }
        #endregion


        #region IConfiguration implementation
        public void SetConfiguration(string fileOrUrl)
        {
            if(!string.IsNullOrEmpty(fileOrUrl))
            {
                if(IsValidStringUrl(fileOrUrl))
                {
                    _setConfiguration(fileOrUrl);
                }
                else
                {
                    _setConfiguration(Path.Combine(Application.streamingAssetsPath, fileOrUrl));
                }
            }
            else
            {
                Debug.LogWarning("Configuration path is null or empty");
            }
        }

        private static bool IsValidStringUrl(string uriName)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        public void Clear()
        {
            _clear();
        }

        public void BeginTransaction()
        {
            _beginTransaction();
        }

        public void EndTransaction()
        {
            _endTransaction();
        }

        public void SetBaseDesign(int width, int height, int ppi, GetSocial.ScaleMode scaleMode)
        {
            _setBaseDesign(width, height, (float) ppi, "scale-with-screen-size");
        }

        public void SetBasePath(string basePath)
        {
            _setBasePath(basePath);
        }
            
        public void SetImagePath(string id, string path)
        {
            _setImagePath(id, path);
        }

        public void SetDimension(string id, float dimension)
        {
            _setDimension(id, dimension);
        }
            
        public void SetAspectRatio(string id, float aspectRatio)
        {
            _setAspectRatio(id, aspectRatio);
        }

        public void SetColor(string id, uint color)
        {
            _setColor(id, color);
        }

        public void SetTextStyle(string id, string typefacePath, float textSize, uint textColor, uint strokeColor = 0u, float strokeSize = 0f, float strokeXOffset = 0f, float strokeYOffset = 0f)
        {
            _setTextStyle(id, typefacePath, textSize, textColor, 
                strokeColor, strokeSize, strokeXOffset, strokeYOffset);
        }

        public void SetAnimationStyle(string id, GetSocial.AnimationStyle animationStyle)
        {
            _setAnimationStyle(id, (int)animationStyle);
        }

        public void SetInsets(string id, int top, int right, int bottom, int left)
        {
            _setInsets(id, top, right, bottom, left);
        }
        #endregion


        #region C# to ObjC method bindings definitions
        [DllImport("__Internal")]
        private static extern void _setConfiguration(string filePath);

        [DllImport("__Internal")]
        private static extern void _clear();

        [DllImport("__Internal")]
        private static extern void _beginTransaction();

        [DllImport("__Internal")]
        private static extern void _endTransaction();

        [DllImport("__Internal")]
        private static extern void _setBasePath(string basePath);

        [DllImport("__Internal")]
        private static extern void _setImagePath(string id, string path);

        [DllImport("__Internal")]
        private static extern void _setBaseDesign(float width, float height, float ppi, string scaleMode);

        [DllImport("__Internal")]
        private static extern void _setDimension(string id, float dimension);

        [DllImport("__Internal")]
        private static extern void _setAspectRatio(string id, float aspectRatio);

        [DllImport("__Internal")]
        private static extern void _setColor(string id, uint color);

        [DllImport("__Internal")]
        private static extern void _setTextStyle(string id, string typefacePath, float textSize, uint textColor,
                                                 uint strokeColor, float strokeSize, float strokeXOffset, float strokeYOffset);

        [DllImport("__Internal")]
        private static extern void _setAnimationStyle(string id, int animationStyle);

        [DllImport("__Internal")]
        private static extern void _setInsets(string id, float top, float right, float bottom, float left);
        #endregion
    }
}
#endif
