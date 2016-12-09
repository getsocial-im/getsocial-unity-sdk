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

namespace GetSocialSdk.Core
{
    internal sealed class ConfigurationAndroid : IConfiguration
    {
        private static IConfiguration instance;
        private readonly AndroidJavaObject configuration;

        #region initialization
        private ConfigurationAndroid(AndroidJavaObject configuration)
        {
            Check.Argument.IsNotNull(configuration, "configuration", "Failed to instantiate Android GetSocial SDK Configuration");
            this.configuration = configuration;
        }

        internal static IConfiguration GetInstance(AndroidJavaObject getSocialJavaObject)
        {
            if(instance == null)
            {
                var configuration = getSocialJavaObject.Call<AndroidJavaObject>("getConfiguration");
                instance = new ConfigurationAndroid(configuration);
            }
            return instance;
        }
        #endregion

        #region IConfiguration implementation
        public void SetConfiguration(string fileOrUrl)
        {
            if(string.IsNullOrEmpty(fileOrUrl))
            {
                Debug.LogWarning("Configuration path is null or empty");
                return;
            }

            configuration.Call("setConfiguration", fileOrUrl);
        }

        public void Clear()
        {
            configuration.Call("clear");
        }

        public void BeginTransaction()
        {
            configuration.Call("beginTransaction");
        }

        public void EndTransaction()
        {
            configuration.Call("endTransaction");
        }

        public void SetBasePath(string baseAssetsPath)
        {
            configuration.Call("setBasePath", baseAssetsPath);
        }

        public void SetImagePath(string id, string path)
        {
            configuration.Call("setImagePath", id, path);
        }

        public void SetBaseDesign(int width, int height, int ppi, GetSocial.ScaleMode scaleMode)
        {
            var scaleModeString = "";
            switch(scaleMode)
            {
                case GetSocial.ScaleMode.ScaleWithScreenSize:
                    scaleModeString = "scale-with-screen-size";
                    break;
                default:
                    Log.W("Specified scale mode is not valid");
                    scaleModeString = "scale-with-screen-size";
                    break;
            }

            configuration.Call("setBaseDesign", width, height, ppi, scaleModeString);
        }

        public void SetDimension(string id, float dimension)
        {
            configuration.Call("setDimension", id, dimension);
        }

        public void SetAspectRatio(string id, float aspectRatio)
        {
            configuration.Call("setAspectRatio", id, aspectRatio);
        }

        public void SetColor(string id, uint color)
        {
            configuration.Call("setColor", id, (int)color);
        }

        public void SetTextStyle(string id, string typefacePath, float textSize, uint textColor, uint strokeColor = 0, float strokeSize = 0f, float strokeXOffset = 0f, float strokeYOffset = 0f)
        {
            configuration.Call("setTextStyle", id, typefacePath, textSize, (int)textColor, (int)strokeColor, strokeSize, strokeXOffset, strokeYOffset);
        }

        public void SetAnimationStyle(string id, GetSocial.AnimationStyle animationStyle)
        {
            configuration.Call("setAnimationStyle", id, (int)animationStyle);
        }

        public void SetInsets(string id, int top, int right, int bottom, int left)
        {
            configuration.Call("setInsets", id, top, right, bottom, left);
        }
        #endregion
    }
}
#endif
