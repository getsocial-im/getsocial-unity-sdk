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

using System.Reflection;

namespace GetSocialSdk.Core
{
    internal class ConfigurationEditorMock : IConfiguration
    {
        private static IConfiguration instance;

        internal static IConfiguration GetInstance()
        {
            if(instance == null)
            {
                instance = new ConfigurationEditorMock();
            }
            return instance;
        }

        #region IConfiguration implementation
        public bool IsInitialized
        {
            get
            {
                return false;
            }
        }

        public void SetConfiguration(string fileOrUrl)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), fileOrUrl);
        }

        public void Clear()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
        }
        
        public void BeginTransaction()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
        }

        public void EndTransaction()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
        }

        public void SetBaseDesign(int width, int height, int ppi, GetSocial.ScaleMode scaleMode)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), width, height, ppi, scaleMode);
        }

        public void SetAnimationStyle(string id, GetSocial.AnimationStyle animationStyle)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), id, animationStyle);
        }

        public void SetDimension(string id, float dimension)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), id, dimension);
        }

        public void SetBasePath(string basePath)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), basePath);
        }

        public void SetColor(string id, uint color)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), id, color);
        }

        public void SetImagePath(string id, string path)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), id, path);
        }

        public void SetTextStyle(string id, string typefacePath, float textSize, uint textColor, uint strokeColor = 0u, float strokeSize = 0f, float strokeXOffset = 0f, float strokeYOffset = 0f)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), id, typefacePath, textSize, textColor, strokeColor, strokeSize, strokeXOffset, strokeYOffset);
        }

        public void SetAspectRatio(string id, float aspectRatio)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), id, aspectRatio);
        }

        public void SetInsets(string id, int top, int right, int bottom, int left)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), id, top, right, bottom, left);
        }
        #endregion
    }
}
